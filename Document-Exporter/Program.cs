using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using NDesk.Options;
using System.Collections.Generic;
using System.IO;

namespace Document_Exporter
{
    class Program
    {
        // Entry Point of the console app
        static void Main(string[] args)
        {
            Helper.SetLastVersionIE();

            var show_help = false;
            var delete = false;
            string file = null;
            string TempPrinter = null;

            var p = new OptionSet() {
            { "f=", "The input file",   v => file = v },
            { "p=", "The temporary default printer",   v => TempPrinter = v },

            // help...
            { "h|help",  "show this message and exit",
              v => show_help = v != null },
            
            { "d|delete",  "delete file when finished",
              v => delete = v != null },
        };
            try
            {
                p.Parse(args);
                if (show_help)
                {
                    p.WriteOptionDescriptions(Console.Out);
                    return;
                }
                if ((file == null) || (TempPrinter == null))
                {
                    return;
                }
            }
            catch (OptionException e)
            {
                Console.Write("bundling: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.");
                return;
            }

            string namePrinter = string.Empty;
            try
            {
                namePrinter = GetDefaultPrinterName();
                SetDefaultPrinter(TempPrinter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set default printer failed: " + ex.Message);
            }

            try
            {
                // download each page and dump the content
                var task = MessageLoopWorker.Run(DoWorkAsync, file);
                task.Wait();
                Console.WriteLine("DoWorkAsync completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DoWorkAsync failed: " + ex.Message);
            }
            try
            {
                SetDefaultPrinter(namePrinter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Set default printer failed: " + ex.Message);
            }
            try
            {
                if (File.Exists(file) && delete)
                    File.Delete(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("File delete error: " + ex.Message);
            }
        }
        public static bool SetDefaultPrinter(string defaultPrinter)
        {
            using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer"))
            {
                using (ManagementObjectCollection objectCollection = objectSearcher.Get())
                {
                    foreach (ManagementObject mo in objectCollection)
                    {
                        if (string.Compare(mo["Name"].ToString(), defaultPrinter, true) == 0)
                        {
                            mo.InvokeMethod("SetDefaultPrinter", null, null);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static string GetDefaultPrinterName()
        {
            var query = new ObjectQuery("SELECT * FROM Win32_Printer");
            var searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                if (((bool?)mo["Default"]) ?? false)
                {
                    return mo["Name"] as string;
                }
            }

            return null;
        }

        static void PrintDocument(string url)
        {
            //WebBrowser web
        }

        // navigate WebBrowser to the list of urls in a loop
        static async Task<object> DoWorkAsync(object[] url)
        {
            Console.WriteLine("Start working.");

            var wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true;

            if (wb.Document == null && wb.ActiveXInstance == null)
                throw new ApplicationException("Unable to initialize the underlying WebBrowserActiveX");

            // get the underlying WebBrowser ActiveX object;
            // this code depends on SHDocVw.dll COM interop assembly,
            // generate SHDocVw.dll: "tlbimp.exe ieframe.dll",
            // and add as a reference to the project
            var wbax = (SHDocVw.WebBrowser)wb.ActiveXInstance;

            TaskCompletionSource<bool> loadedTcs = null;
            WebBrowserDocumentCompletedEventHandler documentCompletedHandler = (s, e) =>
                loadedTcs.TrySetResult(true); // turn event into awaitable task

            TaskCompletionSource<bool> printedTcs = null;
            SHDocVw.DWebBrowserEvents2_PrintTemplateTeardownEventHandler printTemplateTeardownHandler = (p) =>
                printedTcs.TrySetResult(true); // turn event into awaitable task

            // navigate to URL
                loadedTcs = new TaskCompletionSource<bool>();
                wb.DocumentCompleted += documentCompletedHandler;
                try
                {
                    wb.Navigate(url[0].ToString());
                    // await for DocumentCompleted
                    await loadedTcs.Task;
                }
                finally
                {
                    //wb.DocumentCompleted -= documentCompletedHandler;
                }

                // the DOM is ready, 
                Console.WriteLine(url.ToString());
                Console.WriteLine(wb.Document.Body.OuterHtml);

                // print the document
                printedTcs = new TaskCompletionSource<bool>();
                wbax.PrintTemplateTeardown += printTemplateTeardownHandler;
                try
                {
                    wb.Print();
                    // await for PrintTemplateTeardown - the end of printing
                    await printedTcs.Task;
                }
                finally
                {
                    wbax.PrintTemplateTeardown -= printTemplateTeardownHandler;
                }
                Console.WriteLine(url.ToString() + " printed.");

            wb.Dispose();
            Console.WriteLine("End working.");
            return null;
        }

    }

    // a helper class to start the message loop and execute an asynchronous task
    public static class MessageLoopWorker
    {
        public static async Task<object> Run(Func<object[], Task<object>> worker, params object[] args)
        {
            var tcs = new TaskCompletionSource<object>();

            var thread = new Thread(() =>
            {
                EventHandler idleHandler = null;

                idleHandler = async (s, e) =>
                {
                    // handle Application.Idle just once
                    Application.Idle -= idleHandler;

                    // return to the message loop
                    await Task.Yield();

                    // and continue asynchronously
                    // propogate the result or exception
                    try
                    {
                        var result = await worker(args);
                        tcs.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }

                    // signal to exit the message loop
                    // Application.Run will exit at this point
                    Application.ExitThread();
                };

                // handle Application.Idle just once
                // to make sure we're inside the message loop
                // and SynchronizationContext has been correctly installed
                Application.Idle += idleHandler;
                Application.Run();
            });

            // set STA model for the new thread
            thread.SetApartmentState(ApartmentState.STA);

            // start the thread and await for the task
            thread.Start();
            try
            {
                return await tcs.Task;
            }
            finally
            {
                thread.Join();
            }
        }
    }
}