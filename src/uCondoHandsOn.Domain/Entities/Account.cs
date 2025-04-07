using System.ComponentModel.DataAnnotations;
using uCondoHandsOn.Domain.Enums;

namespace uCondoHandsOn.Domain.Entities
{
    public class Account : IComparable<Account>
    {
        [Key]
        public required string Code { get; set; }
        public required string Name { get; set; }
        public AccountType Type { get; set; } 
         public Account? Parent { get; set; }
          public string? ParentCode { get; set; }
         public bool AllowEntries { get; set; }
        public IEnumerable<Account> Children { get; set; } = new List<Account>();
        
        /// <summary>
        /// Compara hierarquicamente dois códigos de conta no formato "1.2.3", tratando cada nível como um número inteiro.
        /// Retorna -1 se o código atual for menor, 1 se for maior e 0 se forem equivalentes.
        /// A comparação é feita de forma segura, utilizando TryParse para validar cada parte do código.
        /// Lógica de comparação de versões usada na implementação de Zip em LINQ.
        /// Referência: https://github.com/dotnet/dotnet/blob/bb1467e123bafa84f854a09910678ac1ed6be3ae/src/runtime/src/libraries/System.Linq.AsyncEnumerable/src/System/Linq/Zip.cs#L27
        /// </summary>
        /// <param name="value">Conta a ser comparada com a instância atual.</param>
        /// <returns>
        /// -1 se o código atual for menor, 1 se for maior, 0 se forem iguais.
        /// </returns>
        public int CompareTo(Account? value)
        {
            var thisCodeParts = ParseCodeParts(Code);
            var otherCodeParts = ParseCodeParts(value.Code);

            foreach (var (a, b) in thisCodeParts.Zip(otherCodeParts, (a, b) => (a, b)))
            {
                var diff = a.CompareTo(b);
                if (diff != 0) return diff;
            }

            return thisCodeParts.Count().CompareTo(otherCodeParts.Count());
        }

        private static List<int> ParseCodeParts(string code)
        {
            var parts = new List<int>();
            var slices = code.Split('.');

            foreach (var slice in slices)
            {
                if (!int.TryParse(slice, out var number))
                    throw new FormatException($"Falha ao interpretar o código: '{code}': o trecho '{slice}' nãe é um inteiro.");
                
                parts.Add(number);
            }

            return parts;
        }

        
    }
}