using System;
using Fjv.Modules.Attributes;
using Fjv.Modules.Generic;

namespace Fjv.Modules.Examples
{
    /// <summary>
    /// Example of a module using the generic interfaces with string input and output
    /// </summary>
    [Module("-string-processor")]
    [ModuleHelp("Process string data with various operations")]
    public class StringProcessorModule : IDefaultModule<string, string>
    {
        private string _content;

        /// <summary>
        /// Loads the module with string input directly
        /// </summary>
        public string Load(string input, string[] args, int index)
        {
            _content = input ?? string.Empty;
            Console.WriteLine($"Loaded string content: {_content.Substring(0, Math.Min(20, _content.Length))}...");
            return _content;
        }

        /// <summary>
        /// Converts the string to uppercase
        /// </summary>
        [Option("--upper")]
        [OptionHelp("Convert the string to uppercase")]
        public string ToUpper()
        {
            var result = _content.ToUpper();
            Console.WriteLine("Text converted to uppercase");
            return result;
        }

        /// <summary>
        /// Converts the string to lowercase
        /// </summary>
        [Option("--lower")]
        [OptionHelp("Convert the string to lowercase")]
        public string ToLower()
        {
            var result = _content.ToLower();
            Console.WriteLine("Text converted to lowercase");
            return result;
        }

        /// <summary>
        /// Extracts a substring
        /// </summary>
        [Option("--substring")]
        [OptionHelp("Extract a substring (startIndex, length)")]
        public string Substring(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex >= _content.Length)
            {
                Console.WriteLine($"Invalid start index: {startIndex}");
                return _content;
            }

            length = Math.Min(length, _content.Length - startIndex);
            var result = _content.Substring(startIndex, length);
            Console.WriteLine($"Extracted substring of length {length} starting at position {startIndex}");
            return result;
        }
    }

    /// <summary>
    /// Example of an argumentable module using generic types with JSON input and output
    /// </summary>
    [Module("-json-processor")]
    [ModuleHelp("Process JSON data")]
    public class JsonProcessorModule : IArgumentableModule<string, string, string>
    {
        private string _jsonContent;
        private string _property;

        /// <summary>
        /// Loads the module with string input and argument
        /// </summary>
        public string Load(string input, string property, string[] args, int index)
        {
            _jsonContent = input ?? "{}";
            _property = property;
            
            Console.WriteLine($"Loaded JSON content with property path: {_property}");
            return _jsonContent;
        }

        /// <summary>
        /// Adds a new property to the JSON
        /// </summary>
        [Option("--add-property")]
        [OptionHelp("Add a new property to the JSON (name, value)")]
        public string AddProperty(string name, string value)
        {
            // In a real implementation, we'd use a JSON library
            // This is just a simple demonstration
            var result = _jsonContent.TrimEnd('}') + $",\"{name}\":\"{value}\"}}";
            Console.WriteLine($"Added property {name} with value {value}");
            return result;
        }
    }
}
