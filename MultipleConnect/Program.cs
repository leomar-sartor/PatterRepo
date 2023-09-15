using MultipleConnect;
using MultipleConnect.Services;

Console.WriteLine("Iniciando!");

var conexao = new Conexao();

long PedidoId = 291;
long EmpresaId = 2;
long UserId = 2;

var service = new ProdutorPedidoService(conexao);
var estornado = service.EstornarPedido(PedidoId, UserId, EmpresaId);

Console.ReadKey();