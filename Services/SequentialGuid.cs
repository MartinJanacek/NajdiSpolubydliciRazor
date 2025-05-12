using Microsoft.EntityFrameworkCore.ValueGeneration;
using NajdiSpolubydliciRazor.Helpers.Interfaces;

namespace NajdiSpolubydliciRazor.Helpers
{
    public class SequentialGuid : ISequentialGuid
    {
        private static readonly SequentialGuidValueGenerator SequentialGuidValueGenerator = new();

        public Guid CreateSequentialGuidForNewEntity()
        {
            return SequentialGuidValueGenerator.Next(null!);
        }
    }
}
