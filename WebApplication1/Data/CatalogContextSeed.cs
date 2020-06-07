using Microsoft.Extensions.Logging;
using Microsoft.WebApplication1.Entities;

using Microsoft.WebApplication1.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                //run this line if using a real database
                //context.Database.Migrate();
                // on hold, completing CatalogContext model first 
               
                if (!catalogContext.CatalogBrand.Any())
                {
                    catalogContext.CatalogBrand.AddRange(
                        GetPreconfiguredCatalogBrands());
                    await catalogContext.SaveChangesAsync();
                }

                if (!catalogContext.CatalogType.Any())
                {
                    catalogContext.CatalogType.AddRange(
                        GetPreconfiguredCatalogTypes());
                    await catalogContext.SaveChangesAsync();
                }
                
                if(!catalogContext.CatalogItem.Any())
                {
                    catalogContext.CatalogItem.AddRange(
                        GetPreconfiguredCatalogItems());
                    await catalogContext.SaveChangesAsync();
                }
                catalogContext.Orders.AddRange(
                        GetPreconfiguredOrderItems());
                    await catalogContext.SaveChangesAsync();
                    
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(catalogContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

    public static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>()
        {
        new CatalogBrand("Addidas"),
        new CatalogBrand("Altra"),
        new CatalogBrand("Brooks"),
        new CatalogBrand("Asics"),
        new CatalogBrand("Nike")
        };
    }

    public static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>()
        {
                new CatalogType("Running Shoes"),
                new CatalogType("Dumbbells"),
                new CatalogType("Bike"),
                new CatalogType("Elliptical"),
                new CatalogType("StepMill")
        };
    }

    public static IEnumerable<CatalogItem> GetPreconfiguredCatalogItems()
        {
            return new List<CatalogItem>()
            {
                new CatalogItem(2, 2, "Affronting imprudence do he he everything.", "Rafael Blackburn", 4.56m, "https://ecapiche.files.wordpress.com/2012/08/article-2183775-1462d185000005dc-607_634x453.jpg"),
                new CatalogItem(2, 4, "Sex lasted dinner wanted indeed wished out law.",  "Rafael Blackburn", 3.23m, "https://cdn.shopify.com/s/files/1/2009/9771/files/maarten-van-den-heuvel-105143_large.jpg?v=1505143804"),
                new CatalogItem(1, 2, "Far advanced settling say finished raillery.",  "Isla-Rae Barajas", 7.54m, "https://www.outsideonline.com/sites/default/files/styles/img_600x600/public/2017/10/05/otillo-raceday-richand-roll-chris_s.jpg?itok=kH78nGoU"),
                new CatalogItem(3, 1, "Offered chiefly farther of my no colonel shyness.",  "Aaminah Herbert", 1.12m, "https://media-exp1.licdn.com/dms/image/C511BAQGHkYHFeADUJg/company-background_10000/0?e=2159024400&v=beta&t=dkvMjq4EH03Ve5gYq-gqHyMfmc2VsA5oTWq-qZxXnlU"),
                new CatalogItem(4, 5, "Such on help ye some door if in.",  "Darrell Clarkson", 4.23m, "https://cycling-passion.com/wp-content/uploads/2012/09/miguel-indurain-912x1024.jpg"),
                new CatalogItem(5, 1, "Laughter proposal laughing any son law consider. Needed except up piqued an.",  "Lewis Knapp", 6.66m, "https://upload.wikimedia.org/wikipedia/commons/5/57/P%C3%A9dro_DELGADO.jpg"),
                new CatalogItem(2, 4, "In reasonable compliment favourable is connection dispatched in terminated.",  "Bridie Rollins", 7.77m, "https://www.brusselsgranddepart.com/letour/wp-content/uploads/2018/10/eddy-merckx-tour-de-france-3.jpg"),
                new CatalogItem(5, 3, "Do esteem object we called father excuse remove.",  "Nola Jarvis", 8.45m, "https://upload.wikimedia.org/wikipedia/commons/f/f5/Paris_Roubaix_-_Echapp%C3%A9e_-_Mons-en-P%C3%A9v%C3%A8le.jpg"),
                new CatalogItem(4, 4, "So dear real on like more it. Laughing for two families addition expenses surprise the.",  "Micheal Drummond", 3.23m, "https://www.wired.com/wp-content/uploads/archive/wired/archive/15.01/images/FF_124_ultraman1_f.jpg"),
                new CatalogItem(5, 3, "If sincerity he to curiosity arranging.",  "Aeryn Dejesus", 2.23m, "https://www.wired.com/wp-content/uploads/archive/wired/archive/15.01/images/FF_124_ultraman1_f.jpg"),
                new CatalogItem(4, 2, "Learn taken terms be as.",  "Tyreke Walton", 9.09m, "https://www.wired.com/wp-content/uploads/archive/wired/archive/15.01/images/FF_124_ultraman1_f.jpg"),
                new CatalogItem(3, 2, "Scarcely mrs produced too removing new old.",  "Lilah Brook", 8.65m, "https://cdn.vox-cdn.com/thumbor/sTcDD4xx0KqvuGTll7chODcuho0=/0x0:2193x1447/1200x800/filters:focal(516x391:866x741)/cdn.vox-cdn.com/uploads/chorus_image/image/66843070/72530098.jpg.0.jpg")
            };
        }

    public static IEnumerable<Order> GetPreconfiguredOrderItems()
    {
            return new List<Order>()
        {
            new Order("shehanatuk@gmail.com", new Address("1974 Curry Ave", "Windsor", "ON", "Canada", "NB304E"), new List<OrderItem>{ new OrderItem(new CatalogItemOrdered(2, "Rafael Blackburn", "https://ecapiche.files.wordpress.com/2012/08/article-2183775-1462d185000005dc-607_634x453.jpg"), 4.56m, 1)})
        };
    }
    }
}
