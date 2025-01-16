exports.Routes = {
	GET: {
		"/"				: "index#index#トップページ",
		"/route"		: "index#route#APIリスト羅列",
		"/favicon.ico"	: "resource#favicon#favicon",
		"/ranking" : {
			"/list"   : "ranking#get#               ランキング取得",
		},
		"/load" : {
			"@hash%s" : "usersave#loadData#         データ取得",
		},
	},
	POST: {
		"/ranking" : {
			"/save"   : "ranking#save#               ランキング登録",
		},
		"/save" : "usersave#saveData#                データ保存",
	}
}

exports.Auth = {
	UseSessionAuth: false,
	PassThroughRoute: {
		GET: [],
		POST: []
	}
};
