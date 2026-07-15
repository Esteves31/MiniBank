namespace MiniBank.Domain.Entities;

public class Cliente
{
    Guid Id { get; set; }
    string Nome { get; set; }
    string Cpf { get; set; }
    DateTime CriadoEm { get; set; }
    ICollection<Conta> Contas { get; set; }
}