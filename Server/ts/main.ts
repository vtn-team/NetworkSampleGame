import { launch } from "./server"
import { findElasticIP } from "./elasticip"
import { launchDGS } from "./gameserver/server"
import { connect } from "./lib/database"
import { HTTP_SERVER_PORT, GAME_SERVER_PORT } from "./config/config"

(async function() {
	//自分のIPを取得する
	findElasticIP();
	
	//HTTPサーバ起動
	launch(HTTP_SERVER_PORT);
	
	//ゲームサーバ起動
	launchDGS(GAME_SERVER_PORT);
})();
