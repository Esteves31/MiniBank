namespace MiniBank.Domain.Entities;

public class Conta
{
    Guid Id { get; set; }
    string Numero { get; set; }
    decimal Saldo { get; set; }
    Guid ClienteId { get; set; }

    public void Depositar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do depósito deve ser maior que zero.");
        }
        Saldo += valor;
    }

    public void Sacar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do saque deve ser maior que zero.");
        }
        if (valor > Saldo)
        {
            throw new InvalidOperationException("Saldo insuficiente.");
        }
        Saldo -= valor;
    }
}