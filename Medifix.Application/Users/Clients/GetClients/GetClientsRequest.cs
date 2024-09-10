using MediFix.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFix.Application.Users.Clients.GetClients;

public record GetClientsRequest : IQuery<ClientsResponse>;