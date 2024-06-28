using Bogus;
using FinancialChat.Application.Entities.Chat;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Tests.Services.Chat
{
    public class SaveMessage : ChatServiceBase
    {
        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(10)]
        public void SaveSomeMessages(int messageAmmount)
        {
            var messages = new Faker<MessagesData>()
                .RuleFor(md => md.From, f => f.Internet.Email())
                .RuleFor(md => md.To, f => f.Lorem.Word())
                .RuleFor(md => md.Created, f => DateTime.Now)
                .RuleFor(md => md.Message, f => f.Lorem.Locale)
                .Generate(messageAmmount);

            foreach (var message in messages)
            {
                _service.SaveMessage(message);
            }

            _repository.Received(messageAmmount).Add(
                Arg.Is<MessagesData>(md => messages.Contains(md)));
        }
    }
}
