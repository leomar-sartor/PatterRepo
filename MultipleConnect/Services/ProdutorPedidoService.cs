using MultipleConnect.Entidades;
using MultipleConnect.Enuns;
using MultipleConnect.Repositorios;

namespace MultipleConnect.Services
{
    public class ProdutorPedidoService
    {
        public IConexao _conexao;

        private readonly PedidoRepository _rPedido;
        private readonly MovimentoRepository _rMovimento;
        private readonly FaturaRepository _rFatura;
        private readonly DuplicataRepository _rDuplicata;
        private readonly PagamentoRepository _rPagamento;
        private readonly CaixaRepository _rCaixa;

        public ProdutorPedidoService(IConexao? conexao = null)
        {
            if (conexao == null)
                _conexao = new Conexao();
            else
                _conexao = conexao;

            _rPedido = new PedidoRepository(_conexao);
            _rMovimento = new MovimentoRepository(_conexao);
            _rFatura = new FaturaRepository(_conexao);
            _rDuplicata = new DuplicataRepository(_conexao);
            _rPagamento = new PagamentoRepository(_conexao);
            _rCaixa = new CaixaRepository(_conexao);
        }

        public bool EstornarPedido(long PedidoId, long UserId, long EmpresaId)
        {
            using (var transaction = _conexao.BeginTransaction())
            {
                Console.WriteLine("Transacionando...");

                try
                {
                    //ESTORNO
                    var pedido = _rPedido.Buscar(PedidoId, transaction);
                    var statusPedido = pedido.Status;

                    if (pedido is not null)
                    {
                        pedido.Status = TipoStatusPedido.Cancelado;
                        pedido.Situacao = TipoSituacaoPedido.Cancelado;
                        _rPedido.Alterar(pedido, transaction);

                        //ForcaErro.DividePorZero();

                        if (statusPedido == TipoStatusPedido.Realizado)
                        {
                            var movimentoBd = _rMovimento.BuscarPorPedido(PedidoId, transaction);

                            var estorno = new Movimento();
                            estorno.Estorno = true;
                            estorno.UserId = UserId;
                            estorno.EmpresaId = EmpresaId;
                            estorno.PedidoId = PedidoId;
                            estorno.LoteId = movimentoBd.LoteId;
                            estorno.Identificador = pedido.LoteIdentificador ?? "";
                            estorno.TalhaoId = movimentoBd.TalhaoId;
                            estorno.LocalId = movimentoBd.LocalId;
                            estorno.ClienteId = movimentoBd.ClienteId;
                            estorno.GramaId = movimentoBd.GramaId;
                            estorno.IntervaloDeCorte = 0;
                            estorno.Quantidade = movimentoBd.Quantidade;
                            estorno.Operacao = TipoMovimento.Entrada;

                            //Calcular o Saldo
                            var ultimoSaldo = _rMovimento.BuscarUltimoMovimento(movimentoBd.LoteId, movimentoBd.TalhaoId, movimentoBd.LocalId, EmpresaId, transaction);
                            if (ultimoSaldo is null)
                            {
                                estorno.SaldoAnterior = 0;
                                estorno.Saldo = estorno.Quantidade;
                            }
                            else
                            {
                                estorno.SaldoAnterior = ultimoSaldo.Saldo;
                                estorno.Saldo = ultimoSaldo.Saldo + estorno.Quantidade;
                            }

                            _rMovimento.Salvar(estorno, transaction);

                            //ForcaErro.DividePorZero();
                        }

                        //Verificar se Pedido tem Fatura, duplicata e pagamento
                        //Fatura 
                        var fatura = _rFatura.BuscarPorPedido(PedidoId, transaction);
                        if (fatura != null)
                        {
                            fatura.Status = TipoFaturaStatus.Cancelada;
                            _rFatura.Salvar(fatura, transaction);

                            //ForcaErro.DividePorZero();

                            //Duplicatas
                            var duplicatas = _rDuplicata.BuscarPorFatura(fatura.Id, transaction).ToList();
                            foreach (var dup in duplicatas)
                            {
                                dup.Status = TipoDuplicataStatus.Cancelada;
                                _rDuplicata.Salvar(dup, transaction);

                                //ForcaErro.DividePorZero();

                                //Pagamentos 
                                var pagamentos = _rPagamento.BuscarPorDuplicata(dup.Id, transaction);
                                foreach (var pag in pagamentos)
                                {
                                    pag.Status = TipoPagamentoStatus.Cancelado;
                                    _rPagamento.Salvar(pag, transaction);

                                    //ForcaErro.DividePorZero();
                                }

                                //Caixa
                                var movs = _rCaixa.BuscarPorPedido(PedidoId, transaction).ToList();
                                foreach (var mov in movs)
                                {
                                    var estornoCaixa = new Caixa();
                                    estornoCaixa.UserId = UserId;
                                    estornoCaixa.EmpresaId = EmpresaId;
                                    estornoCaixa.FormaPagamentoId = mov.FormaPagamentoId;
                                    estornoCaixa.NumeroParcela = mov.NumeroParcela;
                                    estornoCaixa.ChequeId = mov.ChequeId;
                                    estornoCaixa.PedidoId = mov.PedidoId;
                                    estornoCaixa.Operacao = mov.Operacao == TipoMovimento.Entrada ? TipoMovimento.Saida : TipoMovimento.Entrada;
                                    estornoCaixa.Estorno = true;
                                    estornoCaixa.Quantidade = mov.Quantidade;
                                    estornoCaixa.ValorUnitario = mov.ValorUnitario;
                                    estornoCaixa.ValorTotal = mov.ValorTotal;
                                    estornoCaixa.Obs = "Estorno por Cancelamento de Pedido";
                                    estornoCaixa.ContaId = mov.ContaId;

                                    //Saldo Geral
                                    var ultimoRegistro = _rCaixa.BuscarUltimo(transaction);
                                    if (ultimoRegistro == null)
                                    {
                                        estornoCaixa.SaldoCaixa = mov.ValorTotal;
                                        estornoCaixa.SaldoCaixaAnterior = 0;
                                    }
                                    else
                                    {
                                        estornoCaixa.SaldoCaixa = ultimoRegistro.SaldoCaixa - mov.ValorTotal;
                                        estornoCaixa.SaldoCaixaAnterior = ultimoRegistro.SaldoCaixa;
                                    }

                                    //Saldo por Conta
                                    var ultimoRegistroConta = _rCaixa.BuscarUltimoPorConta(mov.ContaId ?? 0, transaction);

                                    if (ultimoRegistroConta == null)
                                    {
                                        estornoCaixa.SaldoConta = mov.ValorTotal;
                                    }
                                    else
                                    {
                                        estornoCaixa.SaldoConta = ultimoRegistroConta.SaldoConta - mov.ValorTotal;
                                    }

                                    _rCaixa.Salvar(estornoCaixa, transaction);

                                    //ForcaErro.DividePorZero();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Treta Mano! - {ex.Message}");
                    Console.WriteLine("Mandar Erro");
                    return false;
                }
                finally
                {
                    Console.WriteLine("Terminando Sempre!");
                }

                return true;
            }
        }
    }
}
