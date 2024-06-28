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
    public class GetChatRoomMessages : ChatServiceBase
    {
        [Theory]
        [InlineData("chatRoom")]
        [InlineData("anotherRoom")]
        [InlineData("MoreOneRoom")]
        public void GetAllMessages(string chatRoom)
        {
            var messages = new Faker<MessagesData>()
                .RuleFor(md => md.From, f => f.Internet.Email())
                .RuleFor(md => md.To, f => chatRoom)
                .RuleFor(md => md.Created, f => DateTime.Now)
                .RuleFor(md => md.Message, f => f.Lorem.Locale)
                .GenerateBetween(1, 50);

            _repository.GetChatRoomMessages(
                Arg.Is<string>(cr => cr.Equals(chatRoom)))
                .Returns(messages);

            var result = _service.GetChatRoomMessages(chatRoom);

            result.Should().BeEquivalentTo(messages);
        }
    }
}
