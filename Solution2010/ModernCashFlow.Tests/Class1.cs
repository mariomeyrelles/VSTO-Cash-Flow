using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ModernCashFlow.Tests
{
    public class Class1
    {
        public dynamic ObterTipoAnonimo()
        {
            return new {ID = 123, Nome = "Usuário 1", Idade = 18};
        }

        [Test]
        public void UsarTipoAnonimo()
        {
            var meuTipoAnonimo = ObterTipoAnonimo();
            Console.WriteLine("ID: " + meuTipoAnonimo.ID);
        }
    }
}
