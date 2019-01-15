using Orleans;
using System.Threading.Tasks;

//put in nuget package and delete from here

namespace Handler.Person
{

        public interface IPersonGrain : IGrainWithStringKey
        {
            Task SayHelloAsync();
        }
    }
