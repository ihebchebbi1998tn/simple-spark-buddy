function SQLiteResult(error, insertId, rowsAffected, rows) {
	this.error = error;
	this.insertId = insertId;
	this.rowsAffected = rowsAffected;
	this.rows = rows;
}

let db;

function runQuery(sql, args, cb, onerror) {
	try {
		let rows = db.exec({sql: sql, rowMode: "object", bind: args});
		const insertId = void 0;
		const rowsAffected = 0;
		const resultSet = new SQLiteResult(null, insertId, rowsAffected, rows);
		cb(resultSet);
	} catch (err) {
		onerror(new SQLiteResult(err));
	}
}

function exec(queries, readOnly, callback) {
	const len = queries.length;
	const results = new Array(len);

	let i = 0;

	function checkDone() {
		if (++i === len) {
			callback(null, results);
		} else {
			doNext();
		}
	}

	function onQueryComplete(i) {
		return function (res) {
			results[i] = res;
			checkDone();
		};
	}

	function doNext() {
		const query = queries[i];
		const sql = query.sql;
		const args = query.args;
		runQuery(sql, args, onQueryComplete(i), callback);
	}

	doNext();
}

self.onmessage = (e) => {
	init.then(() => {
		let message = e.data;
		exec(message.queries, message.readOnly, (err, results) => {
			postMessage({callback: message.callback, err: err, results: results});
		});
	})
}

const log = function (level, message) {
	postMessage({
		type: "log", level: level, message: message,
	});
};

const start = function (sqlite3) {
	return new Promise((resolve, reject) => {
		const capi = sqlite3.capi; /*C-style API*/
		log("debug", "sqlite3 version " + capi.sqlite3_libversion() + " " + capi.sqlite3_sourceid());
		sqlite3.installOpfsSAHPoolVfs().then((poolUtil)=>{
			if (poolUtil instanceof Error) {
				reject(poolUtil);
			} else {
				db = new poolUtil.OpfsSAHPoolDb("/" + this.name + ".sqlite3");
				resolve();
			}
		}).catch(reject);
	});
}

importScripts("sqlite3.js");

const init = new Promise((resolve, reject) => {
	self
		.sqlite3InitModule({
			print: message => log("info", message), printErr: error => log("error", error),
		})
		.then(function (sqlite3) {
			start(sqlite3).then(() => {
				resolve();
			}).catch(e => {
				log("error", e.message);
				reject(e);
			});
		});
});