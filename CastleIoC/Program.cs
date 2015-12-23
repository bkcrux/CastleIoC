using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace CastleIoC
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();

            container.Install(new ShoppingInstaller());

            var shopper = container.Resolve<Shopper>();
            shopper.Charge();
            Console.WriteLine(shopper.ChargesForCurrentCard);

            var shopper2 = container.Resolve<Shopper>();
            shopper2.Charge();
            Console.WriteLine(shopper2.ChargesForCurrentCard);

            Console.ReadLine();

        }
    }

    internal class ShoppingInstaller: IWindsorInstaller
    {
        public ShoppingInstaller()
        {
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<Shopper>().LifeStyle.Transient);
            container.Register(Component.For<ICreditCard>().ImplementedBy<MasterCard>().LifeStyle.Transient);
        }
    }

    public class Visa : ICreditCard
    {
        public string Charge()
        {
            ChargeCount++;
            return "Charging with the Visa";
        }

        public int ChargeCount { get; set; }

    }

    public class MasterCard : ICreditCard
    {
        public string Charge()
        {
            ChargeCount++;
            return "Charging with the MasterCard";
        }

        public int ChargeCount { get; set; }
    }

    public interface ICreditCard
    {
        string Charge();
        int ChargeCount { get; set; }
    }

    public class Shopper
    {
        public ICreditCard CreditCard { get; set;}

        public int ChargesForCurrentCard
        {
            get { return this.CreditCard.ChargeCount; }
        }


        public void Charge()
        {
            Console.WriteLine(CreditCard.Charge());
        }
    }

}
