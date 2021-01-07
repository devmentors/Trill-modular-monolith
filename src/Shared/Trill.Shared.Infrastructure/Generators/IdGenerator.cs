using System;
using Trill.Shared.Abstractions.Generators;

namespace Trill.Shared.Infrastructure.Generators
{
    internal class IdGenerator : IIdGenerator
    {
        private readonly IdGen.IdGenerator _generator;

        public IdGenerator()
        {
            var generatorId = 0;
            var generatorIdEnv = Environment.GetEnvironmentVariable("GENERATOR_ID");
            if (!string.IsNullOrWhiteSpace(generatorIdEnv))
            {
                int.TryParse(generatorIdEnv, out generatorId);
            }
            
            _generator = new IdGen.IdGenerator(generatorId);
        }

        public long Generate() => _generator.CreateId();
    }
}