using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Mac), IsUnique = true)]
public class MacAdress
{
    public int Id { get; set; }
    public string? Mac { get; set; }
    public DateTime expirationDate { get; set; }
    public string? Email { get; set; }
}

public class MacAdressValidator : AbstractValidator<MacAdress>
{
    public MacAdressValidator()
    {
        RuleFor(x => x.Mac)
        .NotEmpty()
        .WithMessage("A mac cím nem lehet üres")
        .Matches("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$")
        .WithMessage("A mac cím nem megfelelő formátumú")
        .Must(x => x != null && x.Length == 17)
        .WithMessage("A mac cím nem megfelelő hosszúságú");

        RuleFor(x => x.expirationDate)
        .NotEmpty()
        .WithMessage("A lejárati dátum nem lehet üres")
        .Must(x => x > System.DateTime.Now)
        .WithMessage("A lejárati dátum nem lehet a múltban");

        RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage("Email megadása kötelező")
        .EmailAddress()
        .WithMessage("Email cím nem megfelelő formátumú");
    }
}

public class UpdateMacAdressDTO
{
    public string? Mac { get; set; }
    public DateTime expirationDate { get; set; }
    public string? Email { get; set; }

    public class Validator : AbstractValidator<UpdateMacAdressDTO>
    {
        public Validator()
        {
            RuleFor(x => x.Mac)
            .NotEmpty()
            .WithMessage("A mac cím nem lehet üres")
            .Matches("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$")
            .WithMessage("A mac cím nem megfelelő formátumú")
            .Must(x => x != null && x.Length == 17)
            .WithMessage("A mac cím nem megfelelő hosszúságú");

            RuleFor(x => x.expirationDate)
            .NotEmpty()
            .WithMessage("A lejárati dátum nem lehet üres")
            .Must(x => x > System.DateTime.Now)
            .WithMessage("A lejárati dátum nem lehet a múltban");

            RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email megadása kötelező")
            .EmailAddress()
            .WithMessage("Email cím nem megfelelő formátumú");
        }
    }
}