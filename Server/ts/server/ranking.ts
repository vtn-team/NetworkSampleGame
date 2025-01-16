const { v4: uuidv4 } = require('uuid')
import { query } from "./../lib/database"

async function getRanking(appName: string)
{
	let ret = [];
	let list = await query("SELECT * FROM ScoreBoard WHERE AppName = ? ORDER BY Score DESC LIMIT 0, 10",[appName]);
	console.log(list);
	console.log(appName);
	for(let i=0; i<list.length; ++i)
	{
		let data:any = {};
		try{
			data = JSON.parse(list[i].OtherData);
		}
		catch(ex)
		{
		}
		data.UserId = list[i].UserId;
		data.UserName = list[i].UserName;
		data.Score = list[i].Score;
		ret.push(data);
	}
	return ret;
}

export async function get(req: any,res: any,route: any)
{
	let ret = await getRanking(route.query.AppName);
	
	return { 
		status: 200,
		ranking: ret,
	};
}

export async function save(req: any,res: any,route: any)
{
	let isError = false;
	if(!route.query.AppName) isError = true;
	if(!route.query.UserId) isError = true;
	if(!route.query.UserName) isError = true;
	if(route.query.Score === undefined) isError = true;
	
	if(isError)
	{
		return { 
			status: 404,
			message: "invalid parameter"
		};
	}
	
	let data = route.query;
	let AppName = data.AppName;
	let UserId = data.UserId;
	let UserName = data.UserName;
	let Score = data.Score;
	
	delete data["AppName"];
	delete data["UserId"];
	delete data["UserName"];
	delete data["Score"];
	
	let savedata = JSON.stringify(data);
	
	await query("INSERT INTO ScoreBoard (AppName, UserId, UserName, Score, OtherData) VALUES (?,?,?,?,?) ON DUPLICATE KEY UPDATE Score = ?, OtherData = ?",[AppName, UserId, UserName, Score, savedata, Score, savedata]);
	let ranking = await getRanking(AppName);
	let rank = await query("SELECT (COUNT(*)+1) as Rank FROM ScoreBoard WHERE AppName = ? AND Score > ?",[AppName, Score]);
	
	console.log(ranking)
	console.log(rank)
	
	data.UserId = UserId;
	data.UserName = UserName;
	data.Score = Score;
	
	return { 
		status: 200,
		you: data,
		rank: Number(rank[0].Rank),
		ranking: ranking
	};
}

