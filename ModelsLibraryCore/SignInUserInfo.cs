﻿using System.ComponentModel.DataAnnotations;

namespace ModelsLibraryCore
{
    public class SignInUserInfo
    {
        /// <summary>
        /// Matrícula do funcionário cadastrado no Tribunal de Justiça.
        /// </summary>
        [Required]
        public string Registration { get; set; }
        /// <summary>
        /// Checa se é a matrícula do Tribunal de Justiça para o acesso registro.
        /// Caso falso, a ratificação será feita pelo CPF.
        /// </summary>
        [Required]
        public bool IsPJERJRegistration { get; set; }
        /// <summary>
        /// Senha do funcionário.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}