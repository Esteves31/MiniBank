using MiniBank.Domain.Enums;
using MiniBank.Domain.Exceptions;

namespace MiniBank.Domain.Entities;

public class Transacao
{
    private Guid Id { get; }
    private Guid ContaId { get; }
    private decimal Valor { get; }
    private DateTime DataHora { get; }
    private TipoTransacao Tipo { get; }

    private Transacao(Guid id, Guid contaId, decimal valor, DateTime dataHora, TipoTransacao tipo)
    {
        if (valor <= 0)
        {
            throw new DomainException("O valor da transação deve ser maior que zero.");
        }

        Id = id;
        ContaId = contaId;
        Valor = valor;
        DataHora = dataHora;
        Tipo = tipo;
    }

    public static Transacao NovoDeposito(Guid contaId, decimal valor) =>
        new Transacao(Guid.NewGuid(), contaId, valor, DateTime.Now, TipoTransacao.Deposito);

    public static Transacao NovoSaque(Guid contaId, decimal valor) =>
        new Transacao(Guid.NewGuid(), contaId, valor, DateTime.Now, TipoTransacao.Saque);
    
}