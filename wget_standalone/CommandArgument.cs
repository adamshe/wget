using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdamAes
{
    public class CommandArgument
    {
        public const string ActionField = "action";
        public const string OriginalFileField = "originalFile";
        public const string EncryptedFileField = "encryptedFile";
        public const string DecryptedFileField = "decryptedFile";
        public const string PasswordField = "pwd";
        private CommandLineArgumentsParser _parser;
        public CommandArgument(CommandLineArgumentsParser parser)
        {
            _parser = parser;
            Password = GetValueOrDefault(parser, PasswordField, "God bless me");
            Action = GetValueOrDefault(parser, ActionField, "e");
            OriginalFile = GetValueOrDefault(parser, OriginalFileField, "");
            EncryptedFileName = GetValueOrDefault(parser, EncryptedFileField, "");
            DecryptedFileName = GetValueOrDefault(parser, DecryptedFileField, "");
        }

        public bool IsEncrypt
        {
            get { return Action == "e"; }
        }

        public string Password { get; set; }

        public string Action { get; set; }

        public string OriginalFile { get; set; }

        public string EncryptedFileName { get; set; }

        public string DecryptedFileName { get; set; }

        private string GetValueOrDefault(CommandLineArgumentsParser parser, string field, string defaultVal)
        {
            return parser[field] ?? defaultVal;
        }
    }
}
