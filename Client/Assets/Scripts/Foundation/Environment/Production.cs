
/// <summary>
/// 本番/開発環境
/// </summary>
public class ProductionEnvironment : IEnvironment
{
    public string APIServerURI => "https://node.vtn-game.com";
}
