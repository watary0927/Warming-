using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;

public class Policies : MonoBehaviour
{
    public struct policy
    {
        public ObscuredString title;//題名
        public ObscuredString main;//タイトル
        public ObscuredInt[] debuff;//デバフ対象
    }

    public  policy[] policies=new policy[]
    {
        new policy{title="植林技術",main="官民一体の開発で発展途上国での植林技術が向上。減っていた森林が復活しつつある。",debuff=new ObscuredInt[]{0,2,3,0 } },
        new policy{title="退耕還林",main="政府が過耕作を禁止し、植生破壊が止まる。穀物と引き換えに森林が復活。",debuff=new ObscuredInt[]{0,1,0,1} },
        new policy{title="治山",main="伝統的な治山工法により、水源涵養機能を回復。はげ山に緑が戻る。",debuff=new ObscuredInt[]{0,0,1,1 } },
        new policy{title="衛星",main="上空の衛星からの森林監視が始まる。森林破壊のモニタリングが強化される。",debuff=new ObscuredInt[]{0,3,13,14} },
        new policy{title="有機栽培",main="有害な肥料を使わず栽培された作物が人気に。消費者の意識も変わる。",debuff=new ObscuredInt[]{0,1,13,0} },
        new policy{title="放牧制限",main="人口爆発した地域への支援により、過放牧する必要がなくなる。",debuff=new ObscuredInt[]{0,1,0,3} },
        new policy{title="森林管理",main="林業従事者による植林や間伐が盛んになり、健全な森林が取り戻される。",debuff=new ObscuredInt[]{0,0,0,0} },
        new policy{title="リサイクル",main="3Rをスローガンに、省資源社会へと向かう。市民の意識改革も図られる。",debuff=new ObscuredInt[]{1,3,4,1} },
        new policy{title="廃掃法改正",main="産業廃棄物について定める法律が改正され、温室効果ガス排出に関し厳格化。",debuff=new ObscuredInt[]{1,15,4,3} },
        new policy{title="廃棄物発電",main="増え続ける廃棄物をエネルギー源に使い、化石燃料の消費を節約。",debuff=new ObscuredInt[]{1,3,3,4} },
        new policy{title="ガス回収",main="廃棄物の埋立場から発生する温室効果ガスの回収がなされるようになる。",debuff=new ObscuredInt[]{1,17,1,17} },
        new policy{title="電気自動車",main="街中に充電スタンドが整備され、電気自動車の台数が跳ね上がる。",debuff=new ObscuredInt[]{2,4,2,1} },
        new policy{title="工場集約",main="世界的企業が、本社近くに子会社や工場を設置。輸送によるロスが激減。",debuff=new ObscuredInt[]{2,4,2,2} },
        new policy{title="自動車減税",main="ハイブリットカーやエコカーにかかる自動車税が減免され、普及に拍車がかかる。",debuff=new ObscuredInt[]{2,2,2,2} },
        new policy{title="バイオ燃料",main="バイオ燃料が広く実用化される。実質的な二酸化炭素排出量が減少する。",debuff=new ObscuredInt[]{2,0,2,1 } },
        new policy{title="航空機革命",main="CFRPによる機体軽量化やエンジン性能向上により、航空機の燃費が改善される。",debuff=new ObscuredInt[]{2,4,4,2} },
        new policy{title="船舶革命",main="水素やアンモニアを用いた脱炭素燃料が多くの船舶に導入される。",debuff=new ObscuredInt[]{2,4,3,2} },
        new policy{title="環境税",main="温室効果ガスの排出に際し、税金がかけられる。政府にとっては一石二鳥。",debuff=new ObscuredInt[]{3,16,14,3} },
        new policy{title="工場法",main="工場建設に際し新たな法が整備され、温室効果ガス排出に規制がかかる。",debuff=new ObscuredInt[]{3,15,14,3} },
        new policy{title="新発電",main="大工場を有する企業が、環境への配慮を示すべく再生可能エネルギーを導入。",debuff=new ObscuredInt[]{3,16,4,3} },
        new policy{title="排出量取引",main="二酸化炭素の排出量を取引できるようになる。排出量を少なくし、収益を上げる企業が現れる。",debuff=new ObscuredInt[]{3,16,3,4} },
        new policy{title="新エネルギー",main="太陽光、風力、地熱、潮力――地球の営力を活かした発電が盛んになる。",debuff=new ObscuredInt[]{4,5,16,10} },
        new policy{title="省エネ化",main="技術の進歩で、人類の使う工業製品が、より少ないエネルギーで動くようになる。",debuff=new ObscuredInt[]{4,4,4,4} },
        new policy{title="IT利用",main="製造業の現場でITが導入され効率化、作業での消費エネルギーが減少。",debuff=new ObscuredInt[]{4,4,4,4} },
        new policy{title="ロス回収",main="従来発生していた排熱などのエネルギーロスの活用が促され、省エネの製造が可能になる。",debuff=new ObscuredInt[]{4,16,3,4} },
        new policy{title="炎上",main="ネット上で有名なインフルエンサーが炎上し、デマを見抜くユーザーが増加。",debuff=new ObscuredInt[]{7,9,7,7} },
        new policy{title="情報提供",main="公的機関からネット上の媒体を通じて情報が発信される。協力的なユーザーが増加。",debuff=new ObscuredInt[]{7,11,9,7} },
        new policy{title="啓蒙活動",main="ネット全体での啓蒙活動により、個人レベルからの省エネが推進される。",debuff=new ObscuredInt[]{7,1,7,1} },
        new policy{title="告発",main="ステークホルダーの内部から告発がなされ、癒着が白日の下に。",debuff=new ObscuredInt[]{8,8,8,8} },
        new policy{title="環境活動家",main="地球環境の悪化を憂える活動家が汚職を突き止め、世界に訴えかける。",debuff=new ObscuredInt[]{8,7,9,8} },
        new policy{title="進出規制",main="利益や資源を目的に発展途上国に進出する企業に、規制がかかる。",debuff=new ObscuredInt[]{8,16,13,8} },
        new policy{title="権限強化",main="多くの国が集まる国際機関で権限が強化され、温暖化防止の推進力が高まる。",debuff=new ObscuredInt[]{10,14,15,10} },
        new policy{title="法的拘束力",main="温暖化防止を目指す協定に法的拘束力が付与され、より強力になる。",debuff=new ObscuredInt[]{10,15,13,10} },
        new policy{title="首脳交代",main="温暖化対策を渋る首脳が交代し、各国に協力的な温暖化対策を推進する。",debuff=new ObscuredInt[]{10,11,14,15} },
        new policy{title="政府の広報",main="陰謀や懐疑論をはっきりと否定する政府の広報が発表される。",debuff=new ObscuredInt[]{11,7,9,11} },
        new policy{title="市民感情",main="情報リテラシーの高まった市民間で、懐疑論への反発が広がる。",debuff=new ObscuredInt[]{11,9,8,7} },
        new policy{title="調査",main="国際機関主導で、大規模な調査が行われる。温暖化の存在はますます確実になった。",debuff=new ObscuredInt[]{11,13,10,11} },
        new policy{title="学会追放",main="温暖化に懐疑的だった専門家が学界から追放される。",debuff=new ObscuredInt[]{11,10,11,10} },
        new policy{title="補助金",main="競争社会における不安の声を受け、温暖化防止を目指す企業への補助金が導入される。",debuff=new ObscuredInt[]{12,16,12,12} },
        new policy{title="労働争議",main="極端な資本主義に労働者が反発、企業は対応を余儀なくされる。",debuff=new ObscuredInt[]{12,16,12,12} },
        new policy{title="CSR",main="企業の社会的責任を問う風潮が強まり、温暖化対策を怠り利益のみを追求する事業者が淘汰される。",debuff=new ObscuredInt[]{12,12,16,12}},
        new policy{title="リーダー失脚",main="経済を優先する強力なリーダーが失脚、温暖化防止を目指す潮流へと社会が傾く。",debuff=new ObscuredInt[]{12,14,12,12} },
        new policy{title="水没",main="海面の上昇により、低地のデルタや一部島嶼部が水没。温暖化の影響が目に見える形に。",debuff=new ObscuredInt[]{13,13,13,13} },
        new policy{title="伝染病",main="世界的な気温上昇で、熱帯特有の伝染病の限界が高緯度へと移る。対策のない国では感染爆発。",debuff=new ObscuredInt[]{13,1,0,13} },
        new policy{title="専門家会議",main="国際・学際的な専門家会議が多く開かれ、温暖化の対策案が多く提案される。",debuff=new ObscuredInt[]{14,14,15,14} },
        new policy{title="民意",main="次の選挙を控えた首脳が、温暖化への危機意識が高まる国民の声を受け、環境政策を強化。",debuff=new ObscuredInt[]{14,15,14,15} },
        new policy{title="圧力",main="世界的に高まる危機感の中、国際協調を目指すべく、環境政策に重点が置かれる。",debuff=new ObscuredInt[]{14,10,15,14} },
        new policy{title="コネ",main="温暖化対策の政策を施行する際に、事前の根回しが功を奏し、多くのステークホルダーが協力的に。",debuff=new ObscuredInt[]{15,14,15,15} },
        new policy{title="罰則",main="具体的な罰則が盛り込まれた温暖化対策の政策が発表される。これは大半の企業に適用された。",debuff=new ObscuredInt[]{16,12,16,12} },
        new policy{title="回収技術向上",main="純度の高い二酸化炭素を、採算の取れるコストで回収する技術が開発される。",debuff=new ObscuredInt[]{17,17,4,17} },
        new policy{title="二酸化炭素の活用",main="二酸化炭素をカーボンニュートラルな燃料や農業に活用する技術が実用化される。",debuff=new ObscuredInt[]{17,0,4,17} },
        new policy{title="CCS",main="二酸化炭素を地中に貯留する技術が普及。空中の二酸化炭素は回収される。",debuff=new ObscuredInt[]{17,17,17,17} },
    };
}
//0-12各コマンド、13危機感14有効性15頻度16企業意識17回収量
//new policy{title="",main="",debuff=new ObscuredInt[]{ } },