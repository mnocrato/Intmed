namespace Infrastructure.Data;

using Domain.Models;
using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Medico> Medicos { get; set; }
    public DbSet<Agenda> Agendas { get; set; }
    public DbSet<Consulta> Consultas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Dia)
                .IsRequired();

            entity.Property(c => c.Horario)
                .IsRequired();

            entity.Property(c => c.DataAgendamento)
                .IsRequired();

            entity.HasOne(c => c.Medico)
                .WithMany(m => m.Consultas)
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Nome)
                .IsRequired();

            entity.Property(m => m.CRM)
                .IsRequired();

            entity.Property(m => m.Email)
                .HasMaxLength(255);

            entity.HasIndex(m => m.CRM)
                .IsUnique();

            entity.HasMany(m => m.Consultas)
                .WithOne(c => c.Medico)
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(m => m.Agendas)
                .WithOne(a => a.Medico)
                .HasForeignKey(a => a.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Agenda>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Dia)
                .IsRequired();

            entity.Property(a => a.Horarios)
                .IsRequired();

            entity.HasOne(a => a.Medico)
                .WithMany(m => m.Agendas)
                .HasForeignKey(a => a.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
