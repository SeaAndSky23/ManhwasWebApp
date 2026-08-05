using System;
using System.Collections.Generic;
using ManhwasWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ManhwasWebApp.Data;

public partial class ManhwasContext : DbContext
{
    public ManhwasContext()
    {
    }

    public ManhwasContext(DbContextOptions<ManhwasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Etiquetum> Etiqueta { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Manhwa> Manhwas { get; set; }

    public virtual DbSet<ManhwaAutor> ManhwaAutors { get; set; }

    public virtual DbSet<Personaje> Personajes { get; set; }

    public virtual DbSet<Titulo> Titulos { get; set; }

    public virtual DbSet<VwDetalleManhwa> VwDetalleManhwas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=MANHWAS;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.IdAutor);

            entity.ToTable("AUTOR");

            entity.Property(e => e.IdAutor).HasColumnName("id_autor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Etiquetum>(entity =>
        {
            entity.HasKey(e => e.IdEtiqueta);

            entity.ToTable("ETIQUETA");

            entity.Property(e => e.IdEtiqueta).HasColumnName("id_etiqueta");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero);

            entity.ToTable("GENERO");

            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Manhwa>(entity =>
        {
            entity.HasKey(e => e.IdManhwa);

            entity.ToTable("MANHWA");

            entity.Property(e => e.IdManhwa).HasColumnName("id_manhwa");
            entity.Property(e => e.AnioFinalizacion).HasColumnName("anio_finalizacion");
            entity.Property(e => e.AnioPublicacion).HasColumnName("anio_publicacion");
            entity.Property(e => e.Calificacion)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("calificacion");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Novela).HasColumnName("novela");
            entity.Property(e => e.NumeroCapitulos).HasColumnName("numero_capitulos");
            entity.Property(e => e.Sinopsis)
                .IsUnicode(false)
                .HasColumnName("sinopsis");
            entity.Property(e => e.UrlPortada)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url_portada");

            entity.HasMany(d => d.IdEtiqueta).WithMany(p => p.IdManhwas)
                .UsingEntity<Dictionary<string, object>>(
                    "ManhwaEtiquetum",
                    r => r.HasOne<Etiquetum>().WithMany()
                        .HasForeignKey("IdEtiqueta")
                        .HasConstraintName("FK_ME_ETIQUETA"),
                    l => l.HasOne<Manhwa>().WithMany()
                        .HasForeignKey("IdManhwa")
                        .HasConstraintName("FK_ME_MANHWA"),
                    j =>
                    {
                        j.HasKey("IdManhwa", "IdEtiqueta");
                        j.ToTable("MANHWA_ETIQUETA");
                        j.IndexerProperty<int>("IdManhwa").HasColumnName("id_manhwa");
                        j.IndexerProperty<int>("IdEtiqueta").HasColumnName("id_etiqueta");
                    });

            entity.HasMany(d => d.IdGeneros).WithMany(p => p.IdManhwas)
                .UsingEntity<Dictionary<string, object>>(
                    "ManhwaGenero",
                    r => r.HasOne<Genero>().WithMany()
                        .HasForeignKey("IdGenero")
                        .HasConstraintName("FK_MG_GENERO"),
                    l => l.HasOne<Manhwa>().WithMany()
                        .HasForeignKey("IdManhwa")
                        .HasConstraintName("FK_MG_MANHWA"),
                    j =>
                    {
                        j.HasKey("IdManhwa", "IdGenero");
                        j.ToTable("MANHWA_GENERO");
                        j.IndexerProperty<int>("IdManhwa").HasColumnName("id_manhwa");
                        j.IndexerProperty<int>("IdGenero").HasColumnName("id_genero");
                    });
        });

        modelBuilder.Entity<ManhwaAutor>(entity =>
        {
            entity.HasKey(e => new { e.IdManhwa, e.IdAutor });

            entity.ToTable("MANHWA_AUTOR");

            entity.Property(e => e.IdManhwa).HasColumnName("id_manhwa");
            entity.Property(e => e.IdAutor).HasColumnName("id_autor");
            entity.Property(e => e.Rol)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("Autor")
                .HasColumnName("rol");

            entity.HasOne(d => d.IdAutorNavigation).WithMany(p => p.ManhwaAutors)
                .HasForeignKey(d => d.IdAutor)
                .HasConstraintName("FK_MA_AUTOR");

            entity.HasOne(d => d.IdManhwaNavigation).WithMany(p => p.ManhwaAutors)
                .HasForeignKey(d => d.IdManhwa)
                .HasConstraintName("FK_MA_MANHWA");
        });

        modelBuilder.Entity<Personaje>(entity =>
        {
            entity.HasKey(e => e.IdPersonaje);

            entity.ToTable("PERSONAJE");

            entity.Property(e => e.IdPersonaje).HasColumnName("id_personaje");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.IdManhwa).HasColumnName("id_manhwa");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Ocupacion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ocupacion");

            entity.HasOne(d => d.IdManhwaNavigation).WithMany(p => p.Personajes)
                .HasForeignKey(d => d.IdManhwa)
                .HasConstraintName("FK_PERSONAJE_MANHWA");
        });

        modelBuilder.Entity<Titulo>(entity =>
        {
            entity.HasKey(e => e.IdTitulo);

            entity.ToTable("TITULO");

            entity.Property(e => e.IdTitulo).HasColumnName("id_titulo");
            entity.Property(e => e.IdManhwa).HasColumnName("id_manhwa");
            entity.Property(e => e.Idioma)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idioma");
            entity.Property(e => e.Titulo1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdManhwaNavigation).WithMany(p => p.Titulos)
                .HasForeignKey(d => d.IdManhwa)
                .HasConstraintName("FK_TITULO_MANHWA");
        });

        modelBuilder.Entity<VwDetalleManhwa>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DetalleManhwa");

            entity.Property(e => e.Calificacion)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("calificacion");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.IdManhwa)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_manhwa");
            entity.Property(e => e.Novela)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("novela");
            entity.Property(e => e.NumeroCapitulos).HasColumnName("numero_capitulos");
            entity.Property(e => e.TituloPrincipal)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Titulo_Principal");
            entity.Property(e => e.UrlPortada)
                 .HasMaxLength(255)
                 .HasColumnName("url_portada");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
