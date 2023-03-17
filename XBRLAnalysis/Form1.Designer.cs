namespace XBRLAnalysis
{
    partial class XBRLAnalysis
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.tv_xbrlList = new System.Windows.Forms.TreeView();
            this.pnl_TopRight = new System.Windows.Forms.Panel();
            this.grpBox_TreeList_xbrl = new System.Windows.Forms.GroupBox();
            this.pnl_TopRight_Top = new System.Windows.Forms.Panel();
            this.pnl_treeView1 = new System.Windows.Forms.Panel();
            this.pnl_treeViewInfo = new System.Windows.Forms.Panel();
            this.lbl_corpCode = new System.Windows.Forms.Label();
            this.checkBox_ExpandTree = new System.Windows.Forms.CheckBox();
            this.pnl_Bottom = new System.Windows.Forms.Panel();
            this.pnl_BottomTop = new System.Windows.Forms.Panel();
            this.groupBox_FilePath = new System.Windows.Forms.GroupBox();
            this.radio_LinkCal = new System.Windows.Forms.RadioButton();
            this.radio_LinkDim = new System.Windows.Forms.RadioButton();
            this.radio_LinkPre = new System.Windows.Forms.RadioButton();
            this.radio_LabelEn = new System.Windows.Forms.RadioButton();
            this.lbl_FilePath2 = new System.Windows.Forms.Label();
            this.txt_xbrlFilePath2 = new System.Windows.Forms.TextBox();
            this.lbl_fileType = new System.Windows.Forms.Label();
            this.radio_LabelKo = new System.Windows.Forms.RadioButton();
            this.radio_XBRL = new System.Windows.Forms.RadioButton();
            this.btn_ConvertXbrl = new System.Windows.Forms.Button();
            this.lbl_FilePath1 = new System.Windows.Forms.Label();
            this.txt_xbrlFilePath1 = new System.Windows.Forms.TextBox();
            this.pnl_BottomMiddle = new System.Windows.Forms.Panel();
            this.btn_InsertFinData = new System.Windows.Forms.Button();
            this.btn_Test2 = new System.Windows.Forms.Button();
            this.btn_Test = new System.Windows.Forms.Button();
            this.txtBox_Console = new System.Windows.Forms.TextBox();
            this.pnl_BottomBot = new System.Windows.Forms.Panel();
            this.btn_ToExcel = new System.Windows.Forms.Button();
            this.btn_ConsoleClear = new System.Windows.Forms.Button();
            this.pnl_Top = new System.Windows.Forms.Panel();
            this.pnl_TopLeft = new System.Windows.Forms.Panel();
            this.grpBox_TreeList_folder = new System.Windows.Forms.GroupBox();
            this.pnl_TopLeft_Top = new System.Windows.Forms.Panel();
            this.pnl_treeViewFolder = new System.Windows.Forms.Panel();
            this.tv_FolderList = new System.Windows.Forms.TreeView();
            this.pnl_treeViewInfo_Folder = new System.Windows.Forms.Panel();
            this.txtBox_folderPath = new System.Windows.Forms.TextBox();
            this.btn_OpenFileDialog3 = new System.Windows.Forms.Button();
            this.checkBox_ExpandTreeFolder = new System.Windows.Forms.CheckBox();
            this.pnl_TopRight.SuspendLayout();
            this.grpBox_TreeList_xbrl.SuspendLayout();
            this.pnl_TopRight_Top.SuspendLayout();
            this.pnl_treeView1.SuspendLayout();
            this.pnl_treeViewInfo.SuspendLayout();
            this.pnl_Bottom.SuspendLayout();
            this.pnl_BottomTop.SuspendLayout();
            this.groupBox_FilePath.SuspendLayout();
            this.pnl_BottomMiddle.SuspendLayout();
            this.pnl_BottomBot.SuspendLayout();
            this.pnl_Top.SuspendLayout();
            this.pnl_TopLeft.SuspendLayout();
            this.grpBox_TreeList_folder.SuspendLayout();
            this.pnl_TopLeft_Top.SuspendLayout();
            this.pnl_treeViewFolder.SuspendLayout();
            this.pnl_treeViewInfo_Folder.SuspendLayout();
            this.SuspendLayout();
            // 
            // tv_xbrlList
            // 
            this.tv_xbrlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_xbrlList.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tv_xbrlList.Location = new System.Drawing.Point(0, 33);
            this.tv_xbrlList.Name = "tv_xbrlList";
            this.tv_xbrlList.Size = new System.Drawing.Size(592, 345);
            this.tv_xbrlList.TabIndex = 1;
            // 
            // pnl_TopRight
            // 
            this.pnl_TopRight.Controls.Add(this.grpBox_TreeList_xbrl);
            this.pnl_TopRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnl_TopRight.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnl_TopRight.Location = new System.Drawing.Point(555, 0);
            this.pnl_TopRight.Name = "pnl_TopRight";
            this.pnl_TopRight.Size = new System.Drawing.Size(598, 398);
            this.pnl_TopRight.TabIndex = 2;
            // 
            // grpBox_TreeList_xbrl
            // 
            this.grpBox_TreeList_xbrl.Controls.Add(this.pnl_TopRight_Top);
            this.grpBox_TreeList_xbrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBox_TreeList_xbrl.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpBox_TreeList_xbrl.Location = new System.Drawing.Point(0, 0);
            this.grpBox_TreeList_xbrl.Name = "grpBox_TreeList_xbrl";
            this.grpBox_TreeList_xbrl.Size = new System.Drawing.Size(598, 398);
            this.grpBox_TreeList_xbrl.TabIndex = 3;
            this.grpBox_TreeList_xbrl.TabStop = false;
            this.grpBox_TreeList_xbrl.Text = "파일";
            // 
            // pnl_TopRight_Top
            // 
            this.pnl_TopRight_Top.Controls.Add(this.pnl_treeView1);
            this.pnl_TopRight_Top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_TopRight_Top.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnl_TopRight_Top.Location = new System.Drawing.Point(3, 17);
            this.pnl_TopRight_Top.Name = "pnl_TopRight_Top";
            this.pnl_TopRight_Top.Size = new System.Drawing.Size(592, 378);
            this.pnl_TopRight_Top.TabIndex = 0;
            // 
            // pnl_treeView1
            // 
            this.pnl_treeView1.Controls.Add(this.tv_xbrlList);
            this.pnl_treeView1.Controls.Add(this.pnl_treeViewInfo);
            this.pnl_treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_treeView1.Location = new System.Drawing.Point(0, 0);
            this.pnl_treeView1.Name = "pnl_treeView1";
            this.pnl_treeView1.Size = new System.Drawing.Size(592, 378);
            this.pnl_treeView1.TabIndex = 2;
            // 
            // pnl_treeViewInfo
            // 
            this.pnl_treeViewInfo.Controls.Add(this.lbl_corpCode);
            this.pnl_treeViewInfo.Controls.Add(this.checkBox_ExpandTree);
            this.pnl_treeViewInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_treeViewInfo.Location = new System.Drawing.Point(0, 0);
            this.pnl_treeViewInfo.Name = "pnl_treeViewInfo";
            this.pnl_treeViewInfo.Size = new System.Drawing.Size(592, 33);
            this.pnl_treeViewInfo.TabIndex = 13;
            // 
            // lbl_corpCode
            // 
            this.lbl_corpCode.AutoSize = true;
            this.lbl_corpCode.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_corpCode.Location = new System.Drawing.Point(458, 9);
            this.lbl_corpCode.Name = "lbl_corpCode";
            this.lbl_corpCode.Size = new System.Drawing.Size(61, 14);
            this.lbl_corpCode.TabIndex = 10;
            this.lbl_corpCode.Text = "기업코드 : ";
            // 
            // checkBox_ExpandTree
            // 
            this.checkBox_ExpandTree.AutoSize = true;
            this.checkBox_ExpandTree.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_ExpandTree.Location = new System.Drawing.Point(9, 8);
            this.checkBox_ExpandTree.Name = "checkBox_ExpandTree";
            this.checkBox_ExpandTree.Size = new System.Drawing.Size(59, 18);
            this.checkBox_ExpandTree.TabIndex = 3;
            this.checkBox_ExpandTree.Text = "펼치기";
            this.checkBox_ExpandTree.UseVisualStyleBackColor = true;
            this.checkBox_ExpandTree.CheckedChanged += new System.EventHandler(this.checkBox_ExpandTree_CheckedChanged);
            // 
            // pnl_Bottom
            // 
            this.pnl_Bottom.Controls.Add(this.pnl_BottomTop);
            this.pnl_Bottom.Controls.Add(this.pnl_BottomMiddle);
            this.pnl_Bottom.Controls.Add(this.pnl_BottomBot);
            this.pnl_Bottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Bottom.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnl_Bottom.Location = new System.Drawing.Point(0, 398);
            this.pnl_Bottom.Name = "pnl_Bottom";
            this.pnl_Bottom.Size = new System.Drawing.Size(1153, 405);
            this.pnl_Bottom.TabIndex = 5;
            // 
            // pnl_BottomTop
            // 
            this.pnl_BottomTop.Controls.Add(this.groupBox_FilePath);
            this.pnl_BottomTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_BottomTop.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnl_BottomTop.Location = new System.Drawing.Point(0, 0);
            this.pnl_BottomTop.Name = "pnl_BottomTop";
            this.pnl_BottomTop.Size = new System.Drawing.Size(1153, 131);
            this.pnl_BottomTop.TabIndex = 10;
            // 
            // groupBox_FilePath
            // 
            this.groupBox_FilePath.Controls.Add(this.radio_LinkCal);
            this.groupBox_FilePath.Controls.Add(this.btn_InsertFinData);
            this.groupBox_FilePath.Controls.Add(this.radio_LinkDim);
            this.groupBox_FilePath.Controls.Add(this.radio_LinkPre);
            this.groupBox_FilePath.Controls.Add(this.radio_LabelEn);
            this.groupBox_FilePath.Controls.Add(this.lbl_FilePath2);
            this.groupBox_FilePath.Controls.Add(this.txt_xbrlFilePath2);
            this.groupBox_FilePath.Controls.Add(this.lbl_fileType);
            this.groupBox_FilePath.Controls.Add(this.radio_LabelKo);
            this.groupBox_FilePath.Controls.Add(this.radio_XBRL);
            this.groupBox_FilePath.Controls.Add(this.lbl_FilePath1);
            this.groupBox_FilePath.Controls.Add(this.txt_xbrlFilePath1);
            this.groupBox_FilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_FilePath.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox_FilePath.Location = new System.Drawing.Point(0, 0);
            this.groupBox_FilePath.Name = "groupBox_FilePath";
            this.groupBox_FilePath.Size = new System.Drawing.Size(1153, 131);
            this.groupBox_FilePath.TabIndex = 8;
            this.groupBox_FilePath.TabStop = false;
            this.groupBox_FilePath.Text = "파일경로";
            // 
            // radio_LinkCal
            // 
            this.radio_LinkCal.AutoSize = true;
            this.radio_LinkCal.Location = new System.Drawing.Point(529, 21);
            this.radio_LinkCal.Name = "radio_LinkCal";
            this.radio_LinkCal.Size = new System.Drawing.Size(78, 18);
            this.radio_LinkCal.TabIndex = 26;
            this.radio_LinkCal.TabStop = true;
            this.radio_LinkCal.Text = "Link(CAL)";
            this.radio_LinkCal.UseVisualStyleBackColor = true;
            this.radio_LinkCal.CheckedChanged += new System.EventHandler(this.radio_LinkCal_CheckedChanged);
            // 
            // radio_LinkDim
            // 
            this.radio_LinkDim.AutoSize = true;
            this.radio_LinkDim.Location = new System.Drawing.Point(435, 21);
            this.radio_LinkDim.Name = "radio_LinkDim";
            this.radio_LinkDim.Size = new System.Drawing.Size(79, 18);
            this.radio_LinkDim.TabIndex = 25;
            this.radio_LinkDim.TabStop = true;
            this.radio_LinkDim.Text = "Link(DIM)";
            this.radio_LinkDim.UseVisualStyleBackColor = true;
            this.radio_LinkDim.CheckedChanged += new System.EventHandler(this.radio_LinkDim_CheckedChanged);
            // 
            // radio_LinkPre
            // 
            this.radio_LinkPre.AutoSize = true;
            this.radio_LinkPre.Location = new System.Drawing.Point(341, 21);
            this.radio_LinkPre.Name = "radio_LinkPre";
            this.radio_LinkPre.Size = new System.Drawing.Size(76, 18);
            this.radio_LinkPre.TabIndex = 24;
            this.radio_LinkPre.TabStop = true;
            this.radio_LinkPre.Text = "Link(PRE)";
            this.radio_LinkPre.UseVisualStyleBackColor = true;
            this.radio_LinkPre.CheckedChanged += new System.EventHandler(this.radio_LinkPre_CheckedChanged);
            // 
            // radio_LabelEn
            // 
            this.radio_LabelEn.AutoSize = true;
            this.radio_LabelEn.Location = new System.Drawing.Point(247, 21);
            this.radio_LabelEn.Name = "radio_LabelEn";
            this.radio_LabelEn.Size = new System.Drawing.Size(79, 18);
            this.radio_LabelEn.TabIndex = 23;
            this.radio_LabelEn.TabStop = true;
            this.radio_LabelEn.Text = "Label(EN)";
            this.radio_LabelEn.UseVisualStyleBackColor = true;
            this.radio_LabelEn.CheckedChanged += new System.EventHandler(this.radio_LabelEn_CheckedChanged);
            // 
            // lbl_FilePath2
            // 
            this.lbl_FilePath2.AutoSize = true;
            this.lbl_FilePath2.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_FilePath2.Location = new System.Drawing.Point(17, 88);
            this.lbl_FilePath2.Name = "lbl_FilePath2";
            this.lbl_FilePath2.Size = new System.Drawing.Size(40, 14);
            this.lbl_FilePath2.TabIndex = 20;
            this.lbl_FilePath2.Text = "파일명";
            // 
            // txt_xbrlFilePath2
            // 
            this.txt_xbrlFilePath2.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_xbrlFilePath2.Location = new System.Drawing.Point(86, 84);
            this.txt_xbrlFilePath2.Name = "txt_xbrlFilePath2";
            this.txt_xbrlFilePath2.Size = new System.Drawing.Size(893, 21);
            this.txt_xbrlFilePath2.TabIndex = 19;
            // 
            // lbl_fileType
            // 
            this.lbl_fileType.AutoSize = true;
            this.lbl_fileType.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_fileType.Location = new System.Drawing.Point(17, 23);
            this.lbl_fileType.Name = "lbl_fileType";
            this.lbl_fileType.Size = new System.Drawing.Size(54, 14);
            this.lbl_fileType.TabIndex = 17;
            this.lbl_fileType.Text = "파일 타입";
            // 
            // radio_LabelKo
            // 
            this.radio_LabelKo.AutoSize = true;
            this.radio_LabelKo.Location = new System.Drawing.Point(153, 21);
            this.radio_LabelKo.Name = "radio_LabelKo";
            this.radio_LabelKo.Size = new System.Drawing.Size(80, 18);
            this.radio_LabelKo.TabIndex = 16;
            this.radio_LabelKo.TabStop = true;
            this.radio_LabelKo.Text = "Label(KO)";
            this.radio_LabelKo.UseVisualStyleBackColor = true;
            this.radio_LabelKo.CheckedChanged += new System.EventHandler(this.radio_LabelKo_CheckedChanged);
            // 
            // radio_XBRL
            // 
            this.radio_XBRL.AutoSize = true;
            this.radio_XBRL.Location = new System.Drawing.Point(86, 21);
            this.radio_XBRL.Name = "radio_XBRL";
            this.radio_XBRL.Size = new System.Drawing.Size(53, 18);
            this.radio_XBRL.TabIndex = 15;
            this.radio_XBRL.TabStop = true;
            this.radio_XBRL.Text = "XBRL";
            this.radio_XBRL.UseVisualStyleBackColor = true;
            this.radio_XBRL.CheckedChanged += new System.EventHandler(this.radio_XBRL_CheckedChanged);
            // 
            // btn_ConvertXbrl
            // 
            this.btn_ConvertXbrl.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ConvertXbrl.Location = new System.Drawing.Point(1041, 110);
            this.btn_ConvertXbrl.Name = "btn_ConvertXbrl";
            this.btn_ConvertXbrl.Size = new System.Drawing.Size(75, 23);
            this.btn_ConvertXbrl.TabIndex = 11;
            this.btn_ConvertXbrl.Text = "변환";
            this.btn_ConvertXbrl.UseVisualStyleBackColor = true;
            this.btn_ConvertXbrl.Visible = false;
            this.btn_ConvertXbrl.Click += new System.EventHandler(this.btn_ConvertXbrl_Click);
            // 
            // lbl_FilePath1
            // 
            this.lbl_FilePath1.AutoSize = true;
            this.lbl_FilePath1.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_FilePath1.Location = new System.Drawing.Point(17, 52);
            this.lbl_FilePath1.Name = "lbl_FilePath1";
            this.lbl_FilePath1.Size = new System.Drawing.Size(58, 14);
            this.lbl_FilePath1.TabIndex = 9;
            this.lbl_FilePath1.Text = "파일경로1";
            // 
            // txt_xbrlFilePath1
            // 
            this.txt_xbrlFilePath1.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_xbrlFilePath1.Location = new System.Drawing.Point(86, 48);
            this.txt_xbrlFilePath1.Name = "txt_xbrlFilePath1";
            this.txt_xbrlFilePath1.Size = new System.Drawing.Size(893, 21);
            this.txt_xbrlFilePath1.TabIndex = 7;
            // 
            // pnl_BottomMiddle
            // 
            this.pnl_BottomMiddle.Controls.Add(this.btn_ConvertXbrl);
            this.pnl_BottomMiddle.Controls.Add(this.btn_Test2);
            this.pnl_BottomMiddle.Controls.Add(this.btn_Test);
            this.pnl_BottomMiddle.Controls.Add(this.txtBox_Console);
            this.pnl_BottomMiddle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_BottomMiddle.Location = new System.Drawing.Point(0, 131);
            this.pnl_BottomMiddle.Name = "pnl_BottomMiddle";
            this.pnl_BottomMiddle.Size = new System.Drawing.Size(1153, 241);
            this.pnl_BottomMiddle.TabIndex = 28;
            // 
            // btn_InsertFinData
            // 
            this.btn_InsertFinData.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_InsertFinData.Location = new System.Drawing.Point(987, 48);
            this.btn_InsertFinData.Name = "btn_InsertFinData";
            this.btn_InsertFinData.Size = new System.Drawing.Size(75, 57);
            this.btn_InsertFinData.TabIndex = 27;
            this.btn_InsertFinData.Text = "재무데이터 추가";
            this.btn_InsertFinData.UseVisualStyleBackColor = true;
            this.btn_InsertFinData.Click += new System.EventHandler(this.btn_InsertFinData_Click);
            // 
            // btn_Test2
            // 
            this.btn_Test2.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Test2.Location = new System.Drawing.Point(948, 170);
            this.btn_Test2.Name = "btn_Test2";
            this.btn_Test2.Size = new System.Drawing.Size(75, 23);
            this.btn_Test2.TabIndex = 15;
            this.btn_Test2.Text = "Test2";
            this.btn_Test2.UseVisualStyleBackColor = true;
            this.btn_Test2.Click += new System.EventHandler(this.btn_Test2_Click);
            // 
            // btn_Test
            // 
            this.btn_Test.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Test.Location = new System.Drawing.Point(1041, 170);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(75, 23);
            this.btn_Test.TabIndex = 5;
            this.btn_Test.Text = "Test";
            this.btn_Test.UseVisualStyleBackColor = true;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // txtBox_Console
            // 
            this.txtBox_Console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBox_Console.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBox_Console.Location = new System.Drawing.Point(0, 0);
            this.txtBox_Console.Multiline = true;
            this.txtBox_Console.Name = "txtBox_Console";
            this.txtBox_Console.ReadOnly = true;
            this.txtBox_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBox_Console.Size = new System.Drawing.Size(1153, 241);
            this.txtBox_Console.TabIndex = 8;
            // 
            // pnl_BottomBot
            // 
            this.pnl_BottomBot.Controls.Add(this.btn_ToExcel);
            this.pnl_BottomBot.Controls.Add(this.btn_ConsoleClear);
            this.pnl_BottomBot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_BottomBot.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnl_BottomBot.Location = new System.Drawing.Point(0, 372);
            this.pnl_BottomBot.Name = "pnl_BottomBot";
            this.pnl_BottomBot.Size = new System.Drawing.Size(1153, 33);
            this.pnl_BottomBot.TabIndex = 11;
            // 
            // btn_ToExcel
            // 
            this.btn_ToExcel.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ToExcel.Location = new System.Drawing.Point(1068, 6);
            this.btn_ToExcel.Name = "btn_ToExcel";
            this.btn_ToExcel.Size = new System.Drawing.Size(75, 23);
            this.btn_ToExcel.TabIndex = 14;
            this.btn_ToExcel.Text = "Excel 출력";
            this.btn_ToExcel.UseVisualStyleBackColor = true;
            this.btn_ToExcel.Click += new System.EventHandler(this.btn_ToExcel_Click);
            // 
            // btn_ConsoleClear
            // 
            this.btn_ConsoleClear.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ConsoleClear.Location = new System.Drawing.Point(987, 6);
            this.btn_ConsoleClear.Name = "btn_ConsoleClear";
            this.btn_ConsoleClear.Size = new System.Drawing.Size(75, 23);
            this.btn_ConsoleClear.TabIndex = 13;
            this.btn_ConsoleClear.Text = "삭제";
            this.btn_ConsoleClear.UseVisualStyleBackColor = true;
            this.btn_ConsoleClear.Click += new System.EventHandler(this.btn_ConsoleClear_Click);
            // 
            // pnl_Top
            // 
            this.pnl_Top.Controls.Add(this.pnl_TopLeft);
            this.pnl_Top.Controls.Add(this.pnl_TopRight);
            this.pnl_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_Top.Location = new System.Drawing.Point(0, 0);
            this.pnl_Top.Name = "pnl_Top";
            this.pnl_Top.Size = new System.Drawing.Size(1153, 398);
            this.pnl_Top.TabIndex = 6;
            // 
            // pnl_TopLeft
            // 
            this.pnl_TopLeft.Controls.Add(this.grpBox_TreeList_folder);
            this.pnl_TopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_TopLeft.Location = new System.Drawing.Point(0, 0);
            this.pnl_TopLeft.Name = "pnl_TopLeft";
            this.pnl_TopLeft.Size = new System.Drawing.Size(555, 398);
            this.pnl_TopLeft.TabIndex = 3;
            // 
            // grpBox_TreeList_folder
            // 
            this.grpBox_TreeList_folder.Controls.Add(this.pnl_TopLeft_Top);
            this.grpBox_TreeList_folder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBox_TreeList_folder.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpBox_TreeList_folder.Location = new System.Drawing.Point(0, 0);
            this.grpBox_TreeList_folder.Name = "grpBox_TreeList_folder";
            this.grpBox_TreeList_folder.Size = new System.Drawing.Size(555, 398);
            this.grpBox_TreeList_folder.TabIndex = 4;
            this.grpBox_TreeList_folder.TabStop = false;
            this.grpBox_TreeList_folder.Text = "폴더";
            // 
            // pnl_TopLeft_Top
            // 
            this.pnl_TopLeft_Top.Controls.Add(this.pnl_treeViewFolder);
            this.pnl_TopLeft_Top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_TopLeft_Top.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnl_TopLeft_Top.Location = new System.Drawing.Point(3, 17);
            this.pnl_TopLeft_Top.Name = "pnl_TopLeft_Top";
            this.pnl_TopLeft_Top.Size = new System.Drawing.Size(549, 378);
            this.pnl_TopLeft_Top.TabIndex = 0;
            // 
            // pnl_treeViewFolder
            // 
            this.pnl_treeViewFolder.Controls.Add(this.tv_FolderList);
            this.pnl_treeViewFolder.Controls.Add(this.pnl_treeViewInfo_Folder);
            this.pnl_treeViewFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_treeViewFolder.Location = new System.Drawing.Point(0, 0);
            this.pnl_treeViewFolder.Name = "pnl_treeViewFolder";
            this.pnl_treeViewFolder.Size = new System.Drawing.Size(549, 378);
            this.pnl_treeViewFolder.TabIndex = 2;
            // 
            // tv_FolderList
            // 
            this.tv_FolderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_FolderList.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tv_FolderList.Location = new System.Drawing.Point(0, 33);
            this.tv_FolderList.Name = "tv_FolderList";
            this.tv_FolderList.Size = new System.Drawing.Size(549, 345);
            this.tv_FolderList.TabIndex = 1;
            this.tv_FolderList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv_FolderList_NodeMouseDoubleClick);
            // 
            // pnl_treeViewInfo_Folder
            // 
            this.pnl_treeViewInfo_Folder.Controls.Add(this.txtBox_folderPath);
            this.pnl_treeViewInfo_Folder.Controls.Add(this.btn_OpenFileDialog3);
            this.pnl_treeViewInfo_Folder.Controls.Add(this.checkBox_ExpandTreeFolder);
            this.pnl_treeViewInfo_Folder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_treeViewInfo_Folder.Location = new System.Drawing.Point(0, 0);
            this.pnl_treeViewInfo_Folder.Name = "pnl_treeViewInfo_Folder";
            this.pnl_treeViewInfo_Folder.Size = new System.Drawing.Size(549, 33);
            this.pnl_treeViewInfo_Folder.TabIndex = 13;
            // 
            // txtBox_folderPath
            // 
            this.txtBox_folderPath.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBox_folderPath.Location = new System.Drawing.Point(65, 5);
            this.txtBox_folderPath.Name = "txtBox_folderPath";
            this.txtBox_folderPath.Size = new System.Drawing.Size(409, 21);
            this.txtBox_folderPath.TabIndex = 28;
            // 
            // btn_OpenFileDialog3
            // 
            this.btn_OpenFileDialog3.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OpenFileDialog3.Location = new System.Drawing.Point(480, 5);
            this.btn_OpenFileDialog3.Name = "btn_OpenFileDialog3";
            this.btn_OpenFileDialog3.Size = new System.Drawing.Size(65, 23);
            this.btn_OpenFileDialog3.TabIndex = 30;
            this.btn_OpenFileDialog3.Text = "폴더선택";
            this.btn_OpenFileDialog3.UseVisualStyleBackColor = true;
            this.btn_OpenFileDialog3.Click += new System.EventHandler(this.btn_OpenFileDialog3_Click);
            // 
            // checkBox_ExpandTreeFolder
            // 
            this.checkBox_ExpandTreeFolder.AutoSize = true;
            this.checkBox_ExpandTreeFolder.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_ExpandTreeFolder.Location = new System.Drawing.Point(9, 8);
            this.checkBox_ExpandTreeFolder.Name = "checkBox_ExpandTreeFolder";
            this.checkBox_ExpandTreeFolder.Size = new System.Drawing.Size(59, 18);
            this.checkBox_ExpandTreeFolder.TabIndex = 3;
            this.checkBox_ExpandTreeFolder.Text = "펼치기";
            this.checkBox_ExpandTreeFolder.UseVisualStyleBackColor = true;
            this.checkBox_ExpandTreeFolder.CheckedChanged += new System.EventHandler(this.checkBox_ExpandTreeFolder_CheckedChanged);
            // 
            // XBRLAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1153, 803);
            this.Controls.Add(this.pnl_Bottom);
            this.Controls.Add(this.pnl_Top);
            this.Name = "XBRLAnalysis";
            this.Text = "XBRLAnalysis";
            this.pnl_TopRight.ResumeLayout(false);
            this.grpBox_TreeList_xbrl.ResumeLayout(false);
            this.pnl_TopRight_Top.ResumeLayout(false);
            this.pnl_treeView1.ResumeLayout(false);
            this.pnl_treeViewInfo.ResumeLayout(false);
            this.pnl_treeViewInfo.PerformLayout();
            this.pnl_Bottom.ResumeLayout(false);
            this.pnl_BottomTop.ResumeLayout(false);
            this.groupBox_FilePath.ResumeLayout(false);
            this.groupBox_FilePath.PerformLayout();
            this.pnl_BottomMiddle.ResumeLayout(false);
            this.pnl_BottomMiddle.PerformLayout();
            this.pnl_BottomBot.ResumeLayout(false);
            this.pnl_Top.ResumeLayout(false);
            this.pnl_TopLeft.ResumeLayout(false);
            this.grpBox_TreeList_folder.ResumeLayout(false);
            this.pnl_TopLeft_Top.ResumeLayout(false);
            this.pnl_treeViewFolder.ResumeLayout(false);
            this.pnl_treeViewInfo_Folder.ResumeLayout(false);
            this.pnl_treeViewInfo_Folder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView tv_xbrlList;
        private System.Windows.Forms.Panel pnl_TopRight;
        private System.Windows.Forms.GroupBox grpBox_TreeList_xbrl;
        private System.Windows.Forms.Panel pnl_Bottom;
        private System.Windows.Forms.GroupBox groupBox_FilePath;
        private System.Windows.Forms.Label lbl_FilePath1;
        private System.Windows.Forms.TextBox txt_xbrlFilePath1;
        private System.Windows.Forms.Button btn_Test;
        private System.Windows.Forms.Button btn_ConvertXbrl;
        private System.Windows.Forms.Panel pnl_TopRight_Top;
        private System.Windows.Forms.CheckBox checkBox_ExpandTree;
        private System.Windows.Forms.Panel pnl_BottomBot;
        private System.Windows.Forms.Button btn_ConsoleClear;
        private System.Windows.Forms.Panel pnl_BottomTop;
        private System.Windows.Forms.Button btn_ToExcel;
        private System.Windows.Forms.Panel pnl_treeView1;
        private System.Windows.Forms.Label lbl_fileType;
        private System.Windows.Forms.RadioButton radio_LabelKo;
        private System.Windows.Forms.RadioButton radio_XBRL;
        private System.Windows.Forms.Label lbl_FilePath2;
        private System.Windows.Forms.TextBox txt_xbrlFilePath2;
        private System.Windows.Forms.Panel pnl_treeViewInfo;
        private System.Windows.Forms.RadioButton radio_LinkCal;
        private System.Windows.Forms.RadioButton radio_LinkDim;
        private System.Windows.Forms.RadioButton radio_LinkPre;
        private System.Windows.Forms.RadioButton radio_LabelEn;
        private System.Windows.Forms.Button btn_Test2;
        private System.Windows.Forms.Button btn_InsertFinData;
        private System.Windows.Forms.TextBox txtBox_Console;
        private System.Windows.Forms.Panel pnl_BottomMiddle;
        private System.Windows.Forms.Panel pnl_Top;
        private System.Windows.Forms.Panel pnl_TopLeft;
        private System.Windows.Forms.GroupBox grpBox_TreeList_folder;
        private System.Windows.Forms.Panel pnl_TopLeft_Top;
        private System.Windows.Forms.Panel pnl_treeViewFolder;
        private System.Windows.Forms.TreeView tv_FolderList;
        private System.Windows.Forms.Panel pnl_treeViewInfo_Folder;
        private System.Windows.Forms.CheckBox checkBox_ExpandTreeFolder;
        private System.Windows.Forms.Button btn_OpenFileDialog3;
        private System.Windows.Forms.TextBox txtBox_folderPath;
        private System.Windows.Forms.Label lbl_corpCode;
    }
}

