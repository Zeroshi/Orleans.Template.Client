using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Handler.Person
{
    public class PersonService : IPersonService
    {
        private readonly IClusterClient _orleansClient;

        PersonService(IClusterClient orleansClient)
        {
            _orleansClient = orleansClient;
        }

        public string SayHello(string name)
        {
            var grain = _orleansClient.GetGrain<IPersonGrain>(name);
            grain.SayHelloAsync();
            return "success";
        }
    }
}
