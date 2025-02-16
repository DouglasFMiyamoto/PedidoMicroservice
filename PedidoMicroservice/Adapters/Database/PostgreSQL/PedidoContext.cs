using PedidoMicroservice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PedidoMicroservice.Adapters.Database.PostgreSQL
{
    public class PedidoContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }

        public PedidoContext(DbContextOptions<PedidoContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do ID gerado automaticamente para Guid
            modelBuilder.Entity<PedidoItem>()
                .HasKey(pi => pi.Id);

            modelBuilder.Entity<PedidoItem>()
                .Property(pi => pi.Id)
                .ValueGeneratedOnAdd()  // Geração automática do GUID
                .HasDefaultValueSql("gen_random_uuid()");  // Utiliza a função do PostgreSQL para gerar GUIDs

            modelBuilder.Entity<PedidoItem>()
                .HasOne(pi => pi.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(pi => pi.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PedidoItem>(entity =>
            {
                entity.Property(pi => pi.Quantidade)
                    .IsRequired();

                entity.Property(pi => pi.Customizacao)
                    .HasMaxLength(200);

                entity.Property(pi => pi.Valor)
                    .IsRequired();

                entity.Property(pi => pi.DataCriacao)
                    .IsRequired();
            });

            // Criar um conversor de valores para DateTime -> UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),  // Salvar como UTC
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Ler como UTC
            );

            // Aplicar o conversor a todas as propriedades DateTime
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime));

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(dateTimeConverter);
                }
            }
        }
    }
}