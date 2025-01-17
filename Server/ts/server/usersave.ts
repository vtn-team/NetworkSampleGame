const fs = require('fs').promises;

export async function loadData(req: any,res: any,route: any)
{
	if(!route.query.SaveKey) return { 
		Status: 404
	};
	
	let read = true;
	try {
		var json = await fs.readFile(`save/${route.query.SaveKey}.json`, 'utf8');
	}catch(ex){
		read = false;
	}
	
	if(!read)
	{
		return { 
			Status: 200,
			GameData: "",
		};
	}
	
	return { 
		Status: 200,
		GameData: json,
	};
}

export async function saveData(req: any,res: any,route: any)
{
	if(!route.query.SaveKey) return { 
		Status: 404
	};
	
	await fs.writeFile(`save/${route.query.SaveKey}.json`, route.query.SaveValue);
	
	return { 
		status: 200
	};
}

