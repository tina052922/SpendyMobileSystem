import svgPaths from "./svg-1l2lq1isp3";
import imgEllipse6 from "figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png";

function Left() {
  return (
    <div className="content-stretch flex gap-[8px] items-center py-[2px] relative shrink-0" data-name="Left">
      <p className="font-['SF_Pro:Medium',sans-serif] font-[510] leading-[normal] relative shrink-0 text-[12px] text-white whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        9:41
      </p>
    </div>
  );
}

function BatteryIcon() {
  return (
    <div className="h-[12px] relative shrink-0 w-[26.5px]" data-name="Battery Icon">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 26.5 12">
        <g id="Battery Icon">
          <g id="Combined Shape" opacity="0.4">
            <mask fill="white" id="path-1-inside-1_1_246">
              <path d={svgPaths.p303e8640} />
            </mask>
            <path d={svgPaths.p29c93100} fill="var(--stroke-0, white)" mask="url(#path-1-inside-1_1_246)" />
          </g>
          <rect fill="var(--fill-0, white)" height="8" id="Capacity" rx="1.5" width="20" x="2" y="2" />
        </g>
      </svg>
    </div>
  );
}

function Right() {
  return (
    <div className="content-stretch flex gap-[4px] items-center py-[5px] relative shrink-0" data-name="Right">
      <div className="h-[10px] relative shrink-0 w-[16.5px]" data-name="Combined Shape">
        <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 16.5 10">
          <g id="Combined Shape">
            <path d={svgPaths.p1ced3f00} fill="var(--fill-0, white)" />
            <path d={svgPaths.p44cab80} fill="var(--fill-0, white)" />
            <path d={svgPaths.p3e431e00} fill="var(--fill-0, white)" />
            <path d={svgPaths.p3a664300} fill="var(--fill-0, white)" />
          </g>
        </svg>
      </div>
      <div className="h-[10px] relative shrink-0 w-[14.053px]" data-name="Combined Shape">
        <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 14.0534 9.99998">
          <g id="Combined Shape">
            <path d={svgPaths.p263a2c00} fill="var(--fill-0, white)" />
            <path d={svgPaths.p412b251} fill="var(--fill-0, white)" />
            <path d={svgPaths.p31425800} fill="var(--fill-0, white)" />
          </g>
        </svg>
      </div>
      <p className="font-['SF_Pro:Medium',sans-serif] font-[510] leading-[14px] relative shrink-0 text-[12px] text-right text-white whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        100%
      </p>
      <BatteryIcon />
    </div>
  );
}

function Group() {
  return (
    <div className="absolute inset-[7.51%_4.83%_87.79%_84.99%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 40 40">
        <g id="Group">
          <path clipRule="evenodd" d={svgPaths.p36f7b580} fill="var(--fill-0, #E7EBEE)" fillRule="evenodd" id="Vector" />
        </g>
      </svg>
    </div>
  );
}

function Group6() {
  return (
    <div className="absolute contents left-[-19px] top-0">
      <div className="absolute bg-gradient-to-b from-[#01143d] from-[9%] h-[361px] left-[-19px] to-[#0335a3] top-0 w-[413px]" />
      <div className="absolute content-stretch flex h-[46px] items-center justify-between left-[-9px] overflow-clip px-[26px] py-[4px] top-0 w-[420px]" data-name="Status bar and Menu bar- iPad">
        <Left />
        <Right />
      </div>
      <div className="absolute left-[17px] size-[40px] top-[64px]">
        <img alt="" className="absolute block max-w-none size-full" height="40" src={imgEllipse6} width="40" />
      </div>
      <Group />
    </div>
  );
}

function Group10() {
  return (
    <div className="absolute h-[3.197px] left-[40px] top-[264px] w-[314px]">
      <div className="absolute inset-[-55.99%_0_-5.99%_0]">
        <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 314.008 5.17874">
          <g id="Group 111">
            <line id="Line 6" stroke="var(--stroke-0, #D9D9D9)" strokeLinecap="round" strokeWidth="4" x1="2.00123" x2="312.001" y1="2.98101" y2="2" />
            <path d="M2 3.17874L234.507 2.26252" id="Line 7" stroke="var(--stroke-0, #43B3EF)" strokeLinecap="round" strokeWidth="4" />
          </g>
        </svg>
      </div>
    </div>
  );
}

function Group11() {
  return (
    <div className="absolute contents left-[28px] top-[178px]">
      <div className="absolute bg-[#3e4e65] h-[114px] left-[28px] rounded-[20px] top-[178px] w-[338px]" />
      <p className="absolute font-['Poppins:Bold',sans-serif] leading-[normal] left-[145px] not-italic text-[18px] text-white top-[195px] whitespace-nowrap">Gaming PC</p>
      <Group10 />
    </div>
  );
}

function Group7() {
  return (
    <div className="absolute contents left-[26px] top-[377px]">
      <div className="absolute bg-[#0335a3] h-[42px] left-[26px] rounded-[30px] top-[377px] w-[164px]" />
      <p className="absolute font-['Poppins:SemiBold',sans-serif] h-[25px] leading-[normal] left-[50px] not-italic text-[18px] text-white top-[385px] w-[116px]">Save Money</p>
    </div>
  );
}

function Group8() {
  return (
    <div className="absolute contents left-[207px] top-[377px]">
      <div className="absolute bg-[#b00] h-[42px] left-[207px] rounded-[30px] top-[377px] w-[164px]" />
      <p className="absolute font-['Poppins:SemiBold',sans-serif] h-[25px] leading-[normal] left-[245px] not-italic text-[18px] text-white top-[385px] w-[101px]">Withdraw</p>
    </div>
  );
}

function Group1() {
  return (
    <div className="absolute contents left-[26px] top-[531.57px]">
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[531.57px] w-[345px]" />
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[531.57px] w-[345px]" />
    </div>
  );
}

function Group4() {
  return (
    <div className="absolute contents left-[26px] top-[473px]">
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[473px] w-[345px]" />
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[473px] w-[345px]" />
    </div>
  );
}

function Group12() {
  return (
    <div className="absolute contents left-[26px] top-[473px]">
      <Group1 />
      <p className="absolute font-['Poppins:Medium',sans-serif] h-[34.327px] leading-[34px] left-[41.68px] not-italic text-[#01143d] text-[14px] top-[539.48px] tracking-[-0.14px] w-[81.95px]">2025-09-18</p>
      <p className="-translate-x-full absolute font-['Poppins:Medium','Noto_Sans:Medium',sans-serif] h-[35px] leading-[34px] left-[357px] text-[14px] text-[red] text-right top-[539px] tracking-[-0.14px] w-[45px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 500" }}>
        -₱150
      </p>
      <Group4 />
      <p className="absolute font-['Poppins:Medium',sans-serif] h-[34.327px] leading-[34px] left-[43.62px] not-italic text-[#01143d] text-[14px] top-[480.23px] tracking-[-0.14px] w-[80.938px]">2025-09-18</p>
      <p className="-translate-x-full absolute font-['Poppins:Medium','Noto_Sans:Medium',sans-serif] h-[34.327px] leading-[34px] left-[356.47px] text-[#05b325] text-[14px] text-right top-[480.23px] tracking-[-0.14px] w-[34.399px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 500" }}>
        ₱200
      </p>
    </div>
  );
}

function Group2() {
  return (
    <div className="absolute contents left-[26px] top-[646.57px]">
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[646.57px] w-[345px]" />
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[646.57px] w-[345px]" />
    </div>
  );
}

function Group5() {
  return (
    <div className="absolute contents left-[26px] top-[588px]">
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[588px] w-[345px]" />
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[588px] w-[345px]" />
    </div>
  );
}

function Group13() {
  return (
    <div className="absolute contents left-[26px] top-[588px]">
      <Group2 />
      <p className="absolute font-['Poppins:Medium',sans-serif] h-[34.327px] leading-[34px] left-[41.68px] not-italic text-[#01143d] text-[14px] top-[654.48px] tracking-[-0.14px] w-[81.95px]">2025-09-16</p>
      <p className="-translate-x-full absolute font-['Poppins:Medium','Noto_Sans:Medium',sans-serif] h-[34.327px] leading-[34px] left-[356.67px] text-[#05b325] text-[14px] text-right top-[654.48px] tracking-[-0.14px] w-[34.399px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 500" }}>
        ₱200
      </p>
      <Group5 />
      <p className="absolute font-['Poppins:Medium',sans-serif] h-[34.327px] leading-[34px] left-[43.62px] not-italic text-[#01143d] text-[14px] top-[595.23px] tracking-[-0.14px] w-[80.938px]">2025-09-17</p>
      <p className="-translate-x-full absolute font-['Poppins:Medium','Noto_Sans:Medium',sans-serif] h-[34.327px] leading-[34px] left-[356.47px] text-[#05b325] text-[14px] text-right top-[595.23px] tracking-[-0.14px] w-[34.399px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 500" }}>
        ₱200
      </p>
    </div>
  );
}

function Group3() {
  return (
    <div className="absolute contents left-[26px] top-[761.57px]">
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[761.57px] w-[345px]" />
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[761.57px] w-[345px]" />
    </div>
  );
}

function Group9() {
  return (
    <div className="absolute contents left-[26px] top-[703px]">
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[703px] w-[345px]" />
      <div className="absolute bg-[rgba(255,255,255,0.8)] h-[45.432px] left-[26px] rounded-[13px] top-[703px] w-[345px]" />
    </div>
  );
}

function Group14() {
  return (
    <div className="absolute contents left-[26px] top-[703px]">
      <Group3 />
      <p className="absolute font-['Poppins:Medium',sans-serif] h-[34.327px] leading-[34px] left-[41.68px] not-italic text-[#01143d] text-[14px] top-[769.48px] tracking-[-0.14px] w-[81.95px]">2025-09-15</p>
      <p className="-translate-x-full absolute font-['Poppins:Medium','Noto_Sans:Medium',sans-serif] h-[34.327px] leading-[34px] left-[356.67px] text-[#05b325] text-[14px] text-right top-[769.48px] tracking-[-0.14px] w-[34.399px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 500" }}>
        ₱200
      </p>
      <Group9 />
      <p className="absolute font-['Poppins:Medium',sans-serif] h-[34.327px] leading-[34px] left-[43.62px] not-italic text-[#01143d] text-[14px] top-[710.23px] tracking-[-0.14px] w-[80.938px]">2025-09-15</p>
      <p className="-translate-x-full absolute font-['Poppins:Medium','Noto_Sans:Medium',sans-serif] h-[34.327px] leading-[34px] left-[356.47px] text-[14px] text-[red] text-right top-[710.23px] tracking-[-0.14px] w-[34.399px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 500" }}>
        -₱50
      </p>
    </div>
  );
}

function Group15() {
  return (
    <div className="absolute contents left-[107px] top-[225px]">
      <p className="absolute font-['SF_Pro:Medium',sans-serif] font-[510] h-[20px] leading-[34px] left-[107px] text-[20px] text-white top-[225px] tracking-[-0.2px] w-[179px]" style={{ fontVariationSettings: "'wdth' 100" }}>
        ₱30,000 / ₱40,000
      </p>
    </div>
  );
}

export default function DrawSave() {
  return (
    <div className="bg-[#e7ebee] relative size-full" data-name="Draw/Save">
      <Group6 />
      <div className="absolute bg-[#1b2b42] h-[741px] left-0 rounded-tl-[30px] rounded-tr-[30px] top-[120px] w-[393px]" />
      <p className="absolute font-['Poppins:SemiBold',sans-serif] leading-[normal] left-[110px] not-italic text-[20px] text-white top-[132px] whitespace-nowrap">Draw / Save Plan</p>
      <div className="absolute flex items-center justify-center left-[17px] size-[30px] top-[132px]" style={{ "--transform-inner-width": "1200", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-90">
          <div className="overflow-clip relative size-[30px]" data-name="keyboard_arrow_down">
            <div className="absolute bottom-[35.83%] left-1/4 right-1/4 top-[33.33%]" data-name="icon">
              <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 15 9.25">
                <path d={svgPaths.p12586a00} fill="var(--fill-0, #E7EBEE)" id="icon" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <Group11 />
      <div className="absolute bg-[#3e4e65] h-[45px] left-[26px] rounded-[30px] top-[316px] w-[343px]" />
      <p className="absolute font-['Poppins:Regular',sans-serif] h-[26px] leading-[normal] left-[48px] not-italic text-[18px] text-[rgba(255,255,255,0.6)] top-[326px] w-[142px]">Enter amount</p>
      <div className="absolute h-[31.17px] left-[322px] top-[323px] w-[32.969px]">
        <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 32.9691 31.1705">
          <ellipse cx="16.4846" cy="15.5852" fill="var(--fill-0, #5F6D81)" id="Ellipse 5" rx="16.4846" ry="15.5852" />
        </svg>
      </div>
      <p className="absolute font-['Inter:Semi_Bold',sans-serif] font-semibold h-[24.677px] leading-[34px] left-[330px] not-italic text-[#a3b5cc] text-[24px] top-[323px] tracking-[-0.24px] w-[16.295px]">₱</p>
      <Group7 />
      <Group8 />
      <Group12 />
      <Group13 />
      <Group14 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[normal] left-[28px] not-italic text-[18px] text-white top-[435px] w-[235px]">Transaction History</p>
      <Group15 />
    </div>
  );
}