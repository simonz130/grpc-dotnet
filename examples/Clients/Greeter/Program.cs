﻿#region Copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Grpc.Core;
using Greet;

namespace Sample.Clients
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = new Channel("localhost:50051", ClientResources.SslCredentials);
            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHello(new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            var replies = client.SayHellos(new HelloRequest { Name = "GreeterClient" });
            while (await replies.ResponseStream.MoveNext(CancellationToken.None))
            {
                Console.WriteLine("Greeting: " + replies.ResponseStream.Current.Message);
            }

            Console.WriteLine("Shutting down");
            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
