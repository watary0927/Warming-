using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class CommandList : MonoBehaviour
{
    public struct Command
    {
        public ObscuredString title;//表示時のタイトル
        public ObscuredString main;//表示時の本文

        public ObscuredInt MyIcon;//表示時の自分のカラー
        public ObscuredInt[] Buff;//1.1倍バフをかけるカラーの種類
        public ObscuredBool SelfDebuff;//自己0.8倍デバフが存在するか否か
        public ObscuredInt DebNum;//使用回数
        public ObscuredInt Year;//クールタイムにかかる時間

        public ObscuredInt PowerE;//環境の威力
        public ObscuredInt PowerD;//危機感の威力
        public ObscuredInt PowerV;//政策の有効性の威力
        public ObscuredInt PowerF;//政策の頻度の威力
        public ObscuredInt PowerC;//企業意識の威力

        public ObscuredBool NowUsed;//現在使用しているか
        public ObscuredBool AlreadyUsed;//使用したことがあるか

        public ObscuredInt[] Roots;//これを使用するために必要なコマンドのナンバー
        public ObscuredInt[] AdAreas;//このコマンドが特に効果を発揮する(*1.2)エリア
        //コマンドメニューを開いた時にやること
        //アイコン、タイトル、本文、相手のアイコン、決定ボタンの表示
        //コマンドを決定した時にやること
        //クールタイムの始動、決定ボタンの無効化
        //コマンドのクールタイムが終わった時にやること
        //バフ対象に対するバフ、環境、有効性、頻度、企業意識に対する演算、決定ボタンの有効化
    };
    public Command[] Commands = new Command[42]{
new Command{ title="焼畑農業", main="草地を焼き払い畑作の土地にする。二酸化炭素がやや増加、危機感がわずかに上昇。草地の面積が減少。" ,
              MyIcon=0,Buff=new ObscuredInt[]{0},SelfDebuff=true,Year=1,PowerE=48,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},DebNum=0,AdAreas=new ObscuredInt[]{ 0, 1, 2, 4, 9, 10, 11, 12, 13, 16 }
          },
new Command{ title = "開発", main = "山を切り崩し更地にする。二酸化炭素がやや増加、危機感がわずかに上昇。山間部の自然が減少。",
              MyIcon=0,Buff=new ObscuredInt[]{1},SelfDebuff=true,Year=1,PowerE=48,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},DebNum=0,AdAreas=new ObscuredInt[]{ 2, 3, 6, 14, 18 }
          },
new Command{  title = "アマゾン放火",
    main = "焼畑農業をしながら森を移動する。二酸化炭素がとても増加、危機感がかなり上昇。アマゾン面積が減少。",
              MyIcon=0,Buff=new ObscuredInt[]{0,2 },SelfDebuff=true,Year=3,PowerE=173,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{0},DebNum=0,AdAreas=new ObscuredInt[]{ 2, 3, 6, 14, 18 }
          },
new Command{ title = "マングローブ",
    main = "マングローブ林を破壊し、エビ養殖池に転換する。二酸化炭素がとても増加、危機感がかなり上昇。",
              MyIcon=0,Buff=new ObscuredInt[]{1,3 },SelfDebuff=true,Year=3,PowerE=173,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{1},DebNum=0,AdAreas=new ObscuredInt[]{ 2, 3, 6, 14, 18 }
          },
new Command{ title = "窒素肥料",
    main = "窒素肥料を大量に使用する。亜酸化窒素がかなり増加、危機感がやや上昇。",
              MyIcon=0,Buff=new ObscuredInt[]{0,1,4},SelfDebuff=false,Year=3,PowerE=144,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{0},AdAreas=new ObscuredInt[]{ 0, 1, 2, 4, 9, 10, 11, 12, 13, 16 }
          },
new Command{ title = "使い捨て商品",
    main = "ビニール袋などの化石燃料由来の使い捨て商品の使用量を増加させる。二酸化炭素がやや増加。",
              MyIcon=1,Buff=new ObscuredInt[]{7},SelfDebuff=false,Year=1,PowerE=32,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12 }
          },
new Command{title = "廃棄物",
    main = "大量消費による廃棄物を増大させる。二酸化炭素がかなり増加。" ,
              MyIcon=1,Buff=new ObscuredInt[]{4},SelfDebuff=false,Year=3,PowerE=134,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{5},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12 }
          },
new Command{  title = "廃棄物処理",
    main = "廃棄物の処理量を増大させる。二酸化炭素がやや増加、メタンがかなり増加。",
              MyIcon=1,Buff=new ObscuredInt[]{0},SelfDebuff=false,Year=5,PowerE=320,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{6},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12 }
          },
new Command{ title = "貨物車",
    main = "トラックなどの貨物車の車両数を増大させる。二酸化炭素がやや増加。",
              MyIcon=2,Buff=new ObscuredInt[]{3,4},SelfDebuff=false,Year=3,PowerE=115,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12 }
          },
new Command{ title = "自家乗用車",
    main = "自家乗用車の車両数を増大させる。二酸化炭素がかなり増加。",
              MyIcon=2,Buff=new ObscuredInt[]{2},SelfDebuff=false,Year=5,PowerE=320,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{8},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12 }
          },
new Command{title = "工場建設",
    main = "生産力の向上の為、工場を建設する。二酸化炭素量がやや増加。" ,
              MyIcon=3,Buff=new ObscuredInt[]{0},SelfDebuff=false,Year=1,PowerE=48,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "過剰生産",
    main = "工場で需要を上回る程に製品を生産する。二酸化炭素がやや増加。",
              MyIcon=3,Buff=new ObscuredInt[]{1,2},SelfDebuff=false,Year=3,PowerE=115,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{10},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "公害",
    main = "製造業者の不慮及び無計画な生産によって公害を引き起こす。二酸化炭素がとても増加。",
              MyIcon=3,Buff=new ObscuredInt[]{2,4},SelfDebuff=false,Year=5,PowerE=288,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{11},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "火力発電",
    main = "化石燃料による火力発電量を増大させる。二酸化炭素がやや増加。",
              MyIcon=4,Buff=new ObscuredInt[]{3},SelfDebuff=false,Year=3,PowerE=134,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "化学工業",
    main = "化学製品の製造過程で二酸化炭素を放出する。二酸化炭素がやや増加。",
              MyIcon=4,Buff=new ObscuredInt[]{0},SelfDebuff=false,Year=3,PowerE=134,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{13},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "鉄鋼生産",
    main = "鉄鋼の生成過程で二酸化炭素を放出する。二酸化炭素がかなり増加。",
              MyIcon=4,Buff=new ObscuredInt[]{2,3},SelfDebuff=false,Year=3,PowerE=115,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{13},AdAreas=new ObscuredInt[]{ 0, 1, 4, 6, 7, 8, 13, 14, 15, 16, 17, 18, 19, 20 }
          },
new Command{ title = "機械工業",
    main = "機械の製造過程で二酸化炭素を放出する。二酸化炭素がかなり増加。",
              MyIcon=4,Buff=new ObscuredInt[]{1,2},SelfDebuff=false,Year=3,PowerE=115,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{15},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "都市開発",
    main = "大規模都市開発の過程で二酸化炭素を放出する。二酸化炭素がとても増加。",
              MyIcon=4,Buff=new ObscuredInt[]{0},SelfDebuff=false,Year=5,PowerE=320,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{14,16},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "電化製品",
    main = "電化製品の製造過程で二酸化炭素を放出する。二酸化炭素がかなり増加。",
              MyIcon=4,Buff=new ObscuredInt[]{0,3},SelfDebuff=false,Year=3,PowerE=115,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{15},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "代替フロン",
    main = "エアコンを稼働させる時に、冷媒として代替フロンを放出する。代替フロンがかなり増加。",
              MyIcon=4,Buff=new ObscuredInt[]{1,4},SelfDebuff=false,Year=3,PowerE=115,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{18},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "採掘",
    main = "化石燃料を採掘する際に、メタンを主とする温室効果ガスを排出する。メタンがわずかに増加。二酸化炭素がわずかに増加。亜酸化窒素がわずかに増加。",
              MyIcon=5,Buff=new ObscuredInt[]{0,3,4},SelfDebuff=false,Year=3,PowerE=96,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ }
          },
new Command{ title = "モデル批判",
    main = "予測に使用しているモデルは不正確なデータを使用していると主張し、既存体型の崩壊を目論む。直後の環境コマンドの影響が半分になる。",
              MyIcon=6,Buff=new ObscuredInt[]{12},SelfDebuff=false,Year=1,PowerE=0,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12 }
          },
new Command{ title = "インターネット",
    main = "温暖化懐疑論を唱える無根拠なサイトを乱立させ、閲覧者を錯覚させる。危機感がやや低下。",
              MyIcon=7,Buff=new ObscuredInt[]{7},SelfDebuff=false,Year=1,PowerE=0,PowerD=300,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12 }
          },
new Command{ title = "SNS",
    main = "SNSや動画配信サイトの著名人を買収し、懐疑論を発信させる。危機感がかなり低下。",
              MyIcon=7,Buff=new ObscuredInt[]{11},SelfDebuff=false,Year=1,PowerE=0,PowerD=450,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{22},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12 }
          },
new Command{ title = "利益団体",
    main = "利益団体を組織し、反温暖化主義の政治家に情報提供を行う。政策の有効性がわずかに低下。",
              MyIcon=8,Buff=new ObscuredInt[]{12},SelfDebuff=false,Year=3,PowerE=0,PowerD=300,PowerV=567,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12 }
          },
new Command{ title = "ロビー活動",
    main = "環境政策の決定権がある政治家と親密な関係を築き、反温暖化政策により決定的な影響を狙う。政策の有効性がやや低下。",
              MyIcon=8,Buff=new ObscuredInt[]{10,11},SelfDebuff=false,Year=3,PowerE=0,PowerD=300,PowerV=486,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{24},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12, 6, 13, 15, 16 }
          },
new Command{ title = "シンクタンク",
    main = "科学者や学術機関に投資して、温暖化を否定するのに有用な情報を集めさせる。政策の有効性がかなり低下。",
              MyIcon=8,Buff=new ObscuredInt[]{11},SelfDebuff=false,Year=3,PowerE=0,PowerD=300,PowerV=567,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{25},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12 }
          },
new Command{ title = "選挙支援",
    main = "票集めや資金提供、政策提言を行い、半強制的に反温暖化政策を公約にさせる。政策の有効性がかなり低下。",
              MyIcon=8,Buff=new ObscuredInt[]{8},SelfDebuff=false,Year=5,PowerE=0,PowerD=300,PowerV=1350,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{26},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12 }
          },
new Command{  title = "癒着",
    main = "政治家に献金を行い、癒着体制を築くことでより具体的な政策を代弁させる。政策の有効性がやや低下。政策の頻度がやや低下。",
              MyIcon=8,Buff=new ObscuredInt[]{7,10},SelfDebuff=false,Year=3,PowerE=0,PowerD=300,PowerV=243,PowerF=540,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{25},AdAreas=new ObscuredInt[]{ 6, 13, 15, 16 }
          },
new Command{ title = "買収",
    main = "裏金によって、利益団体と政治家は一蓮托生となった。保身のためにも全力で温暖化を否定する。政策の有効性がとても低下。政策の頻度がとても低下。",
              MyIcon=8,Buff=new ObscuredInt[]{11},SelfDebuff=false,Year=5,PowerE=0,PowerD=300,PowerV=675,PowerF=1500,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{28},AdAreas=new ObscuredInt[]{ 6, 13, 15, 16 }
          },
new Command{ title = "マスメディア",
    main = "マスメディアに温暖化懐疑論を報道させる。危機感が0になるがマスメディアへの信頼が低下し、以降の危機感が与える影響力が大きくなる。",
              MyIcon=9,Buff=new ObscuredInt[]{7,8,10},SelfDebuff=true,Year=1,PowerE=0,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},DebNum=0,AdAreas=new ObscuredInt[]{6, 13, 15, 16}
          },
new Command{ title = "会議妨害",
    main = "議事進行を遅延させる。政策の頻度がわずかに低下。",
              MyIcon=10,Buff=new ObscuredInt[]{7},SelfDebuff=false,Year=1,PowerE=0,PowerD=0,PowerV=0,PowerF=300,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "対立煽動",
    main = "国家間の対立を煽動し、国際的な条約の締結を困難にする。政策の頻度がやや低下。",
              MyIcon=10,Buff=new ObscuredInt[]{8,11},SelfDebuff=false,Year=3,PowerE=0,PowerD=300,PowerV=0,PowerF=1260,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{31},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "協定離脱",
    main = "一部の国家が国際協定から離脱する。政策の頻度がとても低下。",
              MyIcon=10,Buff=new ObscuredInt[]{12},SelfDebuff=false,Year=5,PowerE=0,PowerD=300,PowerV=0,PowerF=3000,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{32},AdAreas=new ObscuredInt[]{ 2, 3, 6, 7, 9, 10, 11, 12, 1, 18 }
          },
new Command{ title = "温暖化否定論",
    main = "温暖化はそもそも発生していないと主張する。危機感がわずかに低下。",
              MyIcon=11,Buff=new ObscuredInt[]{8},SelfDebuff=true,Year=3,PowerE=0,PowerD=1260,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},DebNum=0,AdAreas=new ObscuredInt[]{ }
          },
new Command{ title = "人為否定論",
    main = "温暖化トレンドは地球の気候サイクルによるものであり、人間活動が原因ではないと主張。危機感がやや低下。",
              MyIcon=11,Buff=new ObscuredInt[]{10},SelfDebuff=true,Year=3,PowerE=0,PowerD=1260,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},DebNum=0,AdAreas=new ObscuredInt[]{ }
          },
new Command{ title = "因果逆転説",
    main = "二酸化炭素濃度の上昇が気温上昇を引き起こすのではなく、気温上昇に伴って二酸化炭素濃度が上昇していると主張。危機感がかなり低下。",
              MyIcon=11,Buff=new ObscuredInt[]{10,12},SelfDebuff=true,Year=5,PowerE=0,PowerD=2700,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{34,35},DebNum=0,AdAreas=new ObscuredInt[]{ }
          },
new Command{ title = "近視眼",
    main = "現在の利益を追求し、未来の温暖化問題を疎かにしがちになる。企業意識がわずかに低下。危機感がわずかに低下。",
              MyIcon=12,Buff=new ObscuredInt[]{7},SelfDebuff=false,Year=1,PowerE=0,PowerD=150,PowerV=0,PowerF=0,PowerC=67,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{},AdAreas=new ObscuredInt[]{2, 3, 5, 9, 10, 11, 12 }
          },
new Command{ title = "市場原理",
    main = "温暖化対策によってコスト競争に勝てなくなるので、環境対策を怠る。企業意識がやや低下。",
              MyIcon=12,Buff=new ObscuredInt[]{8},SelfDebuff=false,Year=3,PowerE=0,PowerD=300,PowerV=0,PowerF=0,PowerC=405,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{37},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12 }
          },
new Command{ title = "経済破綻論",
    main = "温暖化対策に対するコストは膨大なため、現在の経済を破壊すると主張。企業意識がかなり低下、政策の頻度がやや低下、危機感がかなり低下。",
              MyIcon=12,Buff=new ObscuredInt[]{8,12},SelfDebuff=false,Year=5,PowerE=0,PowerD=900,PowerV=0,PowerF=900,PowerC=405,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{38},AdAreas=new ObscuredInt[]{ 2, 3, 5, 9, 10, 11, 12 }
          },
new Command{ title = "南北問題",
    main = "北半球諸国と南半球諸国の間の経済格差により、統一的な規制が設けづらくなる。政策の頻度が0になるが、以降の政策の頻度がより急激に上昇する。",
              MyIcon=12,Buff=new ObscuredInt[]{0,10},SelfDebuff=true,Year=1,PowerE=0,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{37},DebNum=0,AdAreas=new ObscuredInt[]{ 0, 1, 4, 6, 7, 8, 13, 14, 15, 16, 17, 18, 19, 20 }
          },
new Command{ title = "南南問題",
    main = "南半球諸国の経済格差により、統一的な規制が設けづらくなる。政策の有効性が0になるが、以降の政策の有効性がより急激に上昇する。",
              MyIcon=12,Buff=new ObscuredInt[]{11,12},SelfDebuff=true,Year=1,PowerE=0,PowerD=0,PowerV=0,PowerF=0,PowerC=0,NowUsed=false,AlreadyUsed=false,Roots=new ObscuredInt[]{40},DebNum=0,AdAreas=new ObscuredInt[]{ 0, 1, 4, 6, 7, 8, 13, 14, 15, 16, 17, 18, 19, 20 }
          }
    };


}
