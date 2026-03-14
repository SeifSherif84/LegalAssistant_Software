using Domain.Entities.ChatBotAIEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class ChatMessageConfigurations : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasOne(chatMessage => chatMessage.ChatSession)
                   .WithMany(chatSession => chatSession.ChatMessages)
                   .HasForeignKey(chatMessage => chatMessage.ChatSessionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
