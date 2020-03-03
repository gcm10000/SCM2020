using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Utility
{
    public class InfoEmployee
    {
        /// <summary>
        /// Matrícula do funcionário cadastrado no Tribunal de Justiça.
        /// </summary>
        public string PJERJRegistration { get; set; }
        /// <summary>
        /// Cadastro de Pessoa Física. Recomendado para usuários que ainda não pertecem a matrícula do PJERJ.
        /// Para usuários sem matrícula o CPF é uma opção para efetuar acesso.
        /// </summary>
        public string CPFRegistration { get; set; }
        /// <summary>
        /// Nome do funcionário.
        /// </summary>
        [Required(ErrorMessage = "Insira o nome.")]
        public string Name { get; set; }
        /// <summary>
        /// Cargo ocupado pelo funcionário.
        /// </summary>
        [Required(ErrorMessage = "Insira o cargo.")]
        public string Occupation { get; set; }
        /// <summary>
        /// Setor do funcionário.
        /// </summary>
        [Required(ErrorMessage = "Insira o setor.")]
        public string Role { get; set; }
    }
}
