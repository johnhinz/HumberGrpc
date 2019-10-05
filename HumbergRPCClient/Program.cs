using Grpc.Net.Client;
using HumberGrpc;
using System;

namespace HumbergRPCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true
                );
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2Support", true);

            GrpcChannel channel = 
                GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Applicant.ApplicantClient(channel);

            var result =
                client.
                GetApplicantProfileAsync(
                    new Id()
                    { Id_ = "1fc2f63c-08b1-7630-3858-00a0e7c57734" })
                .GetAwaiter().GetResult();

            Console.WriteLine(result.Login);
        }
    }
}
