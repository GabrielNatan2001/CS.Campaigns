namespace CS.Campaigns.Application.Messaging;

public class DoacaoRecebidaPublisherConfig
{
    public string Exchange { get; set; } = "cs.doacao.recebida";
    public string RoutingKey { get; set; } = "donations.doacao-recebida";
}
