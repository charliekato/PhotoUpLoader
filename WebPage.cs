class WebPage {
    public const string UploadPage = """
<!DOCTYPE html> <html>  <head>
    <meta charset="utf-8">
    <style>

body {
    color: #222;
    font-family: Helvetica;
    font-size: 30px;
    margin: 0;
    padding: 0;
    text-align:center;
}
h1 {
    margin: 0;
    padding: 0;
}

p {
    font-size: 40px;
}

button, input[type="submit"] {
    font-size:40px;
}

#file {
    display: none;
}
.select-button {
    font-size:40px;
    padding: 12px 24px;
}
.submit-button {
    font-size: 28px;
    padding: 12px 24px;
}


#header h2 {
    text-align: center;
    font-size: 40px;
}

#header form {
    text-align: center;
}


header h1 {
    font-size: 10px;
    color: #fff;
    position: absolute;
    top: 10px;
    left:10px;
}

header .imhere {
	background-color: royalblue;
	color: white;
}
aside {
	display: none;
}
article {
	width: 160%;
}
.hidelink {
    display: none;
}

header nav {
    width: 160%;

    display: flex;
    justify-content: space-between;
}


    </style>


</head>
<body>
<h2>写真UpLoad</h2>
<form method="post" action="/upload" enctype="multipart/form-data">
    <label class="select-button">

    写真を選択
    <input type="file" id="file" name="files" multiple accept="image/*">
    </label>
    <br><br>
    <div id="filelist"></div>
    <input type="submit" class="subit-button" id="sendBtn" value="PCへコピー">
</form>

<p id ="filename" ></p>
<script>
document.getElementById("file").addEventListener("change", function() {
    const list = document.getElementById("filelist");
    const sendBtn = document.getElementById("sendBtn");
    list.innerHTML = "";
    if (this.files.length === 0) return;

    /* === ① 合計サイズ計算 === */
    let total = 0;
    for (const f of this.files) {
        total += f.size;   // bytes
    }
    const MAX_SIZE = 50 * 1024 * 1024; // 50 MB

    const totalMB = (total / 1024 / 1024).toFixed(1);

    /* === ② 枚数表示 === */
    const count = document.createElement("div");
    count.textContent = `選択された写真：${this.files.length} 枚`;
    count.style.fontWeight = "bold";
    list.appendChild(count);

    /* === ③ 合計サイズ表示 === */
    const size = document.createElement("div");
    size.textContent = `合計サイズ：${totalMB} MB`;
    size.style.marginBottom = "8px";
    list.appendChild(size);
    // ▼ サイズ超過チェック
if (total > MAX_SIZE) {
    const error = document.createElement("div");
    error.textContent = "サイズが大きすぎて転送できません";
    error.style.color = "red";
    error.style.fontWeight = "bold";
    list.appendChild(error);
    sendBtn.disabled=true; 
    return; // ここで処理を止める
}

    sendBtn.disabled=false;
    /* === ④ ファイル名一覧 === */
    const ul = document.createElement("ul");
    for (const file of this.files) {
        const li = document.createElement("li");
        li.textContent = file.name;
        ul.appendChild(li);
    }
    list.appendChild(ul);
});
</script>
</body>
</html>
        
""";
   }

