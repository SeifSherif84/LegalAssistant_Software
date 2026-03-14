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
    public class ChatSessionConfigurations : IEntityTypeConfiguration<ChatSession>
    {
        public void Configure(EntityTypeBuilder<ChatSession> builder)
        {
            builder.HasOne(chatSession => chatSession.Lawyer)
                   .WithMany(L => L.ChatSessions)
                   .HasForeignKey(chatSession => chatSession.LawyerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
