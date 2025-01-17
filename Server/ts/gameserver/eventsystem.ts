
export enum TARGET {
	ALL = 0,
	SELF = 1,
	OTHER = 2,	//TargetSessionIdsが渡される
};

export enum CMD {
	WELCOME = 1,
	JOIN = 2,
	EVENT = 3,
	SEND_JOIN = 100,
	SEND_EVENT = 101,
};

export function createMessage(senderId: string, command: CMD, target:TARGET, data: any) {
	//let msg = msgpack.pack(data);
	delete data["Command"]
	let msg = JSON.stringify(data);
	let ret = { 
		"UserId" : senderId,
		"Target" : target,
		"Command" : command,
		"Data" :msg
	};
	return ret;
}


//NOTE: ゲーム同士でイベントのやり取りをする
export class EventSystem {
	sessionDic: any;
	broadcast: any;

	constructor(bc: any) {
		this.sessionDic = {};
		this.broadcast = bc;
	}
	
	parsePayload(payload: any) {
		let data:any = {};
		let types:any = {};
		if(payload) {
			for(var d of payload) {
				types[d.Key] = d.TypeName;
				
				switch(d.TypeName)
				{
				case "Integer":
					data[d.Key] = Number(d.Data);
					break;
					
				case "String":
					data[d.Key] = d.Data;
					break;
					
				default:
					data[d.Key] = d.Data;
					break;
				}
			}
		}
		return {
			data: data,
			types: types
		};
	}
	
	createdPayload(data: any) {
		let payload:any = [];
		if(data) {
			for(var k in data) {
				payload.push({
					Key: k,
					TypeName: typeof data[k],
					Data: data[k]
				});
			}
		}
		return payload;
	}

	public execMessage(data: any) {
		let payload = this.parsePayload(data["Payload"]);
		
		switch(data["Command"])
		{
		case CMD.SEND_JOIN:
			this.joinRoom(data);
			break;
		
		case CMD.SEND_EVENT:
		{
			switch(data.EventId)
			{
			case 100:
			{
				this.messageRelay(data);
				return;
			}
			break;
			}
			
			this.execCommand(data);
			this.broadcast(createMessage(data.UserId, CMD.EVENT, TARGET.ALL, data));
		}
		break;
		}
	}

	joinRoom(data: any) {
		let UserId = data.SessionId;
		this.broadcast(createMessage(UserId, CMD.EVENT, TARGET.OTHER, {
			EventId: 1,
			Payload: this.createdPayload({
				UserId : data.UserId,
				UserName: data.UserName
			})
		}));
		
		//他の人もJoinしていく
		for(let sessionId in this.sessionDic)
		{
			let d = this.sessionDic[sessionId];
			this.broadcast(createMessage(UserId, CMD.EVENT, TARGET.SELF, {
				EventId: 1,
				Payload: this.createdPayload({
					UserId : d.UserId,
					UserName: d.UserName
				})
			}));
			console.log(`USER ID:${UserId} - ${d.UserName} send join.`);
		}
		
		this.sessionDic[UserId] = {
			UserId : data.UserId,
			UserName: data.UserName
		};
		console.log(`USER ID:${data.UserId} - ${data.UserName} join.`);
	}
	
	public removeSession(sessionId: string) {
		if(!this.sessionDic[sessionId]) {
			return;
		}
		
		delete this.sessionDic[sessionId];
		console.log(`USER ID:${sessionId} leave.`);
	}
	
	//処理
	execCommand(data: any) {
		
	}
	
	async messageRelay(data: any) {
		try {
			let json = JSON.parse(data.Data);
			let to = json.ToUserId;
			let from = json.FromUserId;
			if(!to) {
				to = -1;
			}
			if(!from) {
				from = -1;
			}
			
			let message = {
				ToUserId: to,
				FromUserId: from,
				Name: json.Name,
				Message: json.Message
			}
			
			this.broadcast(createMessage(from, CMD.EVENT, TARGET.ALL, message));
		}catch(ex){
			console.log(ex);
		}
	}
};
