using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchInNetCore.Web
{
    public class OrderHelper
    {
        private static List<string> DefinedSearchParams = new List<string>() {
            "Detail",
            "Order Amount",
            "Order #"
        };

        private static string searchString = "";
        private static List<string> searchParameters = new List<string>();
        public static void Insert()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("elasticdemo");
            var client = new ElasticClient(settings);

            var orderDate = Convert.ToDateTime("2019-09-26");
            var order = new OrderSearch
            {
                OrderId = 1,
                OrderDate = orderDate,
                OrderAmount = 1000,
                OrderStatus = "Completed",
                OrderDetails = new List<OrderDetailSearch>()
                {
                    new OrderDetailSearch
                    {
                        OrderDetailId= 1,
                        Detail="divison price",
                        Price=500
                    },
                    new OrderDetailSearch
                    {
                        OrderDetailId= 1 + 1,
                        Detail="divison price",
                        Price=500
                    }
                },
                OrderPayments = new List<OrderPaymentSearch>()
                {
                    new OrderPaymentSearch
                    {
                        OrderPaymentHistoryId = 1,
                        PaymentAmount = 1000,
                        PaymentDate = orderDate.AddMinutes(1),
                        PaymentMethod = "Card",
                        PaymentStatus = "Completed"
                    }
                }
            };
            var asyncIndexResponse = Task.Run(async () => await client.IndexDocumentAsync(order)).Result;
        }
        public static string GetOrders(string searchString)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("elasticdemo");
            var client = new ElasticClient(settings);

            return SearchData(client, searchString);
        }

        private static string SearchData(ElasticClient client, string search)
        {
            List<string> selectedParams = new List<string>() {
                "1"
            };
            for (int i = 0; i < selectedParams.Count; i++)
            {
                searchParameters.Add(DefinedSearchParams[Convert.ToInt32(selectedParams[i]) - 1]);
            }
            searchString = "*" + search + "*";
            return Search(client);
        }

        private static string Search(ElasticClient client)
        {
            SearchDescriptor<OrderSearch> searchDescriptor = new SearchDescriptor<OrderSearch>();
            searchDescriptor.From(0)
                .Size(10)
                .Query(Query);
            var requestjson = client.RequestResponseSerializer.SerializeToString(searchDescriptor);
            var searchResponse = client.Search<OrderSearch>(searchDescriptor);
            var orders = searchResponse.Documents;
            return JsonConvert.SerializeObject(orders.ToArray());
        }

        private static QueryContainer Query(QueryContainerDescriptor<OrderSearch> q)
        {
            return q.QueryString(m => m
                                .Fields(Fields)
                                .Query(searchString));
        }

        private static FieldsDescriptor<OrderSearch> Fields(FieldsDescriptor<OrderSearch> f)
        {
            var newFields = new FieldsDescriptor<OrderSearch>();
            string[] allFields = new string[searchParameters.Count];
            for (int i = 0; i < searchParameters.Count; i++)
            {
                switch (searchParameters[i])
                {
                    case "Detail":
                        newFields.Field(p => p.OrderDetails.First().Detail);
                        break;
                    case "Order Amount":
                        newFields.Field(p => p.OrderAmount);
                        break;
                    case "Order #":
                        newFields.Field(p => p.OrderId);
                        break;
                    default:
                        break;
                }
            }
            return newFields;
        }

    }
}
