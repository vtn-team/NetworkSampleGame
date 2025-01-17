import { getConnectionAddress } from "./../gameserver/server"

//接続アドレスの取得
export async function getaddr(req: any,res: any,route: any)
{
	let addrInfo = getConnectionAddress();
	if(addrInfo == null) {
		return {
			status: 200,
			address: ""
		};
	}
	
	let ret = `ws://${addrInfo.host}:${addrInfo.port}`;
	
	return {
		Status: 200,
		Address: ret
	};
}
