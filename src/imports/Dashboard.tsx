import svgPaths from "./svg-me2nw1cn3q";
import imgEllipse3 from "figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png";
import imgCalendarClock1 from "figma:asset/98f02780778c732f236a1392374cead2bdb61226.png";
import imgRectangle106 from "figma:asset/5aa2dba646fa4b1d5ac2510bb2e96d3cd62d4614.png";
import imgRectangle107 from "figma:asset/6f35f047681a0f54721d817c0de34838c5589b0a.png";
import imgRectangle108 from "figma:asset/1168208e4d9efd47fb1219e2552c19bdb3d11a17.png";
import imgRectangle109 from "figma:asset/184718e0c87fed565c40269ab917c082d4c7a79c.png";

function Group() {
  return (
    <div className="absolute inset-[7.28%_5.09%_88.03%_84.73%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 40 40">
        <g id="Group">
          <path clipRule="evenodd" d={svgPaths.p36f7b580} fill="var(--fill-0, #E7EBEE)" fillRule="evenodd" id="Vector" />
        </g>
      </svg>
    </div>
  );
}

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
            <mask fill="white" id="path-1-inside-1_5_660">
              <path d={svgPaths.p303e8640} />
            </mask>
            <path d={svgPaths.p29c93100} fill="var(--stroke-0, white)" mask="url(#path-1-inside-1_5_660)" />
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

function Group19() {
  return (
    <div className="absolute contents left-[24px] top-[76px]">
      <div className="absolute bg-white h-[52px] left-[24px] rounded-[20px] top-[76px] w-[348px]" />
      <p className="absolute font-['Poppins:Regular',sans-serif] h-[39.289px] leading-[34px] left-[40.33px] not-italic text-[#01143d] text-[14px] top-[85.52px] tracking-[-0.14px] w-[120.776px]">Total Expenditure</p>
    </div>
  );
}

function Group41() {
  return (
    <div className="absolute contents left-[23px] top-[15px]">
      <div className="absolute bg-[#022268] h-[51px] left-[23px] rounded-[30px] top-[15px] w-[172px]" />
      <p className="absolute font-['Poppins:SemiBold',sans-serif] leading-[normal] left-[61px] not-italic text-[20px] text-white top-[26px] w-[114px]">Expenses</p>
    </div>
  );
}

function Group22() {
  return (
    <div className="absolute contents left-[333px] top-[153px]">
      <div className="absolute left-[333px] size-[30px] top-[153px]">
        <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 30 30">
          <circle cx="15" cy="15" fill="var(--fill-0, #01143D)" id="Ellipse 1" r="14.5" stroke="var(--stroke-0, white)" />
        </svg>
      </div>
      <div className="absolute left-[339px] size-[18px] top-[159px]" data-name="calendar-clock 1">
        <img alt="" className="absolute inset-0 max-w-none object-cover pointer-events-none size-full" src={imgCalendarClock1} />
      </div>
    </div>
  );
}

function Group42() {
  return (
    <button className="absolute contents cursor-pointer left-[200px] top-[15px]">
      <div className="absolute bg-[rgba(62,78,101,0.9)] h-[51px] left-[200px] rounded-[30px] top-[15px] w-[172px]" />
      <p className="absolute font-['Poppins:SemiBold',sans-serif] leading-[normal] left-[247px] not-italic text-[20px] text-white top-[26px] w-[94px]">Income</p>
    </button>
  );
}

function Group7() {
  return (
    <div className="absolute contents left-[11px] top-px">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-px w-[348px]" />
    </div>
  );
}

function Group23() {
  return (
    <div className="absolute contents left-[23px] top-[9px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[9px]" />
    </div>
  );
}

function Group1() {
  return (
    <div className="absolute inset-[7.36%_6.27%_6.85%_6.27%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 17.4933 17.1585">
        <g id="Group">
          <path d={svgPaths.p2afaffc0} fill="var(--fill-0, white)" id="Vector" />
          <path d={svgPaths.p2f8095f0} fill="var(--fill-0, white)" id="Vector_2" />
          <path d={svgPaths.p7cd9300} fill="var(--fill-0, white)" id="Vector_3" />
          <path d={svgPaths.p26364500} fill="var(--fill-0, white)" id="Vector_4" />
          <path d={svgPaths.p28402200} fill="var(--fill-0, white)" id="Vector_5" />
          <path d={svgPaths.p1bb57e80} fill="var(--fill-0, white)" id="Vector_6" />
          <path d={svgPaths.pbb3f900} fill="var(--fill-0, white)" id="Vector_7" />
          <path d={svgPaths.p29577c80} fill="var(--fill-0, white)" id="Vector_8" />
        </g>
      </svg>
    </div>
  );
}

function ChickenBurgerRecipesIconSvgCo() {
  return (
    <div className="absolute left-[28px] overflow-clip shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] size-[20px] top-[14px]" data-name="Chicken Burger Recipes - iconSvg.co">
      <Group1 />
    </div>
  );
}

function Group24() {
  return (
    <div className="absolute contents left-[23px] top-[9px]">
      <Group23 />
      <ChickenBurgerRecipesIconSvgCo />
    </div>
  );
}

function Group20() {
  return (
    <div className="absolute contents left-[11px] top-0">
      <Group7 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62.03px] not-italic text-[#01143d] text-[14px] top-[7px] tracking-[-0.14px] w-[35.718px]">Food</p>
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[343.69px] text-[14px] text-[red] text-right top-0 tracking-[-0.14px] w-[33.677px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱80
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[17px] tracking-[-0.14px] w-[33.677px]">12:00</p>
      <Group24 />
    </div>
  );
}

function Group8() {
  return (
    <div className="absolute contents left-[11px] top-[57px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[57px] w-[348px]" />
    </div>
  );
}

function Group26() {
  return (
    <div className="absolute contents left-[23px] top-[65px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[65px]" />
    </div>
  );
}

function Group2() {
  return (
    <div className="absolute inset-[22.19%_87.37%_73.83%_8%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 17.3729 13.1125">
        <g id="Group">
          <path d={svgPaths.p899ff80} fill="var(--fill-0, white)" id="Vector" />
          <path d={svgPaths.p1c15500} fill="var(--fill-0, white)" id="Vector_2" />
        </g>
      </svg>
    </div>
  );
}

function Group25() {
  return (
    <div className="absolute contents left-[23px] top-[65px]">
      <Group26 />
      <Group2 />
    </div>
  );
}

function Group21() {
  return (
    <div className="absolute contents left-[11px] top-[56px]">
      <Group8 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[63px] tracking-[-0.14px] w-[57px]">Traffic</p>
      <Group25 />
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[343.69px] text-[14px] text-[red] text-right top-[56px] tracking-[-0.14px] w-[33.677px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱80
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[73px] tracking-[-0.14px] w-[33.677px]">13:00</p>
    </div>
  );
}

function Group9() {
  return (
    <div className="absolute contents left-[11px] top-[113px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[113px] w-[348px]" />
    </div>
  );
}

function Group28() {
  return (
    <div className="absolute contents left-[23px] top-[121px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[121px]" />
    </div>
  );
}

function Group48() {
  return (
    <div className="absolute contents left-[23px] top-[121px]">
      <Group28 />
      <div className="absolute inset-[38.91%_87.66%_56.39%_8%]" data-name="Vector">
        <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 16.2572 15.4701">
          <path clipRule="evenodd" d={svgPaths.pda50e80} fill="var(--fill-0, white)" fillRule="evenodd" id="Vector" />
        </svg>
      </div>
    </div>
  );
}

function Group27() {
  return (
    <div className="absolute contents left-[11px] top-[112px]">
      <Group9 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[119px] tracking-[-0.14px] w-[83px]">Shopping</p>
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[344px] text-[14px] text-[red] text-right top-[112px] tracking-[-0.14px] w-[58px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱1,530
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[129px] tracking-[-0.14px] w-[33.677px]">13:50</p>
      <Group48 />
    </div>
  );
}

function Group10() {
  return (
    <div className="absolute contents left-[11px] top-[169px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[169px] w-[348px]" />
    </div>
  );
}

function Group30() {
  return (
    <div className="absolute contents left-[23px] top-[177px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[177px]" />
    </div>
  );
}

function Group3() {
  return (
    <div className="absolute inset-[55.93%_87.66%_39.37%_8%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 16.2572 15.4702">
        <g id="Group">
          <path d={svgPaths.p1d140000} fill="var(--fill-0, white)" id="Vector" />
        </g>
      </svg>
    </div>
  );
}

function Group49() {
  return (
    <div className="absolute contents left-[23px] top-[177px]">
      <Group30 />
      <Group3 />
    </div>
  );
}

function Group29() {
  return (
    <div className="absolute contents left-[11px] top-[168px]">
      <Group10 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[175px] tracking-[-0.14px] w-[57px]">Grocery</p>
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[344px] text-[14px] text-[red] text-right top-[168px] tracking-[-0.14px] w-[56px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱3,250
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[185px] tracking-[-0.14px] w-[33.677px]">14:00</p>
      <Group49 />
    </div>
  );
}

function Group11() {
  return (
    <div className="absolute contents left-[11px] top-[224px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[224px] w-[348px]" />
    </div>
  );
}

function Group33() {
  return (
    <div className="absolute contents left-[23px] top-[232px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[232px]" />
    </div>
  );
}

function Group4() {
  return (
    <div className="absolute inset-[72.95%_87.37%_23.07%_8%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 17.3729 13.1125">
        <g id="Group">
          <path d={svgPaths.p899ff80} fill="var(--fill-0, white)" id="Vector" />
          <path d={svgPaths.p1c15500} fill="var(--fill-0, white)" id="Vector_2" />
        </g>
      </svg>
    </div>
  );
}

function Group32() {
  return (
    <div className="absolute contents left-[23px] top-[232px]">
      <Group33 />
      <Group4 />
    </div>
  );
}

function Group31() {
  return (
    <div className="absolute contents left-[11px] top-[223px]">
      <Group11 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[230px] tracking-[-0.14px] w-[57px]">Traffic</p>
      <Group32 />
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[343.69px] text-[14px] text-[red] text-right top-[223px] tracking-[-0.14px] w-[33.677px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱80
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[240px] tracking-[-0.14px] w-[33.677px]">14:30</p>
    </div>
  );
}

function Group12() {
  return (
    <div className="absolute contents left-[11px] top-[279px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[279px] w-[348px]" />
    </div>
  );
}

function Group36() {
  return (
    <div className="absolute contents left-[23px] top-[287px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[287px]" />
    </div>
  );
}

function Group5() {
  return (
    <div className="absolute inset-[7.36%_6.27%_6.85%_6.27%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 17.4933 17.1585">
        <g id="Group">
          <path d={svgPaths.p2afaffc0} fill="var(--fill-0, white)" id="Vector" />
          <path d={svgPaths.p2f8095f0} fill="var(--fill-0, white)" id="Vector_2" />
          <path d={svgPaths.p7cd9300} fill="var(--fill-0, white)" id="Vector_3" />
          <path d={svgPaths.p26364500} fill="var(--fill-0, white)" id="Vector_4" />
          <path d={svgPaths.p28402200} fill="var(--fill-0, white)" id="Vector_5" />
          <path d={svgPaths.p1bb57e80} fill="var(--fill-0, white)" id="Vector_6" />
          <path d={svgPaths.pbb3f900} fill="var(--fill-0, white)" id="Vector_7" />
          <path d={svgPaths.p29577c80} fill="var(--fill-0, white)" id="Vector_8" />
        </g>
      </svg>
    </div>
  );
}

function ChickenBurgerRecipesIconSvgCo1() {
  return (
    <div className="absolute left-[28px] overflow-clip shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] size-[20px] top-[292px]" data-name="Chicken Burger Recipes - iconSvg.co">
      <Group5 />
    </div>
  );
}

function Group35() {
  return (
    <div className="absolute contents left-[23px] top-[287px]">
      <Group36 />
      <ChickenBurgerRecipesIconSvgCo1 />
    </div>
  );
}

function Group34() {
  return (
    <div className="absolute contents left-[11px] top-[278px]">
      <Group12 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62.03px] not-italic text-[#01143d] text-[14px] top-[285px] tracking-[-0.14px] w-[35.718px]">Food</p>
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[343.69px] text-[14px] text-[red] text-right top-[278px] tracking-[-0.14px] w-[33.677px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱80
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[295px] tracking-[-0.14px] w-[33.677px]">12:00</p>
      <Group35 />
    </div>
  );
}

function Group13() {
  return (
    <div className="absolute contents left-[11px] top-[335px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[335px] w-[348px]" />
    </div>
  );
}

function Group39() {
  return (
    <div className="absolute contents left-[23px] top-[343px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[343px]" />
    </div>
  );
}

function Group6() {
  return (
    <div className="absolute inset-[106.69%_87.37%_-10.67%_8%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 17.3729 13.1125">
        <g id="Group">
          <path d={svgPaths.p899ff80} fill="var(--fill-0, white)" id="Vector" />
          <path d={svgPaths.p1c15500} fill="var(--fill-0, white)" id="Vector_2" />
        </g>
      </svg>
    </div>
  );
}

function Group38() {
  return (
    <div className="absolute contents left-[23px] top-[343px]">
      <Group39 />
      <Group6 />
    </div>
  );
}

function Group37() {
  return (
    <div className="absolute contents left-[11px] top-[334px]">
      <Group13 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[341px] tracking-[-0.14px] w-[57px]">Traffic</p>
      <Group38 />
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[343.69px] text-[14px] text-[red] text-right top-[334px] tracking-[-0.14px] w-[33.677px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱80
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[351px] tracking-[-0.14px] w-[33.677px]">13:00</p>
    </div>
  );
}

function Group14() {
  return (
    <div className="absolute contents left-[11px] top-[391px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[391px] w-[348px]" />
    </div>
  );
}

function Group43() {
  return (
    <div className="absolute contents left-[23px] top-[399px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[399px]" />
    </div>
  );
}

function Group51() {
  return (
    <div className="absolute contents left-[23px] top-[399px]">
      <Group43 />
      <div className="absolute inset-[123.71%_87.66%_-28.41%_8%]" data-name="Vector">
        <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 16.2572 15.4701">
          <path clipRule="evenodd" d={svgPaths.pda50e80} fill="var(--fill-0, white)" fillRule="evenodd" id="Vector" />
        </svg>
      </div>
    </div>
  );
}

function Group40() {
  return (
    <div className="absolute contents left-[11px] top-[390px]">
      <Group14 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[397px] tracking-[-0.14px] w-[83px]">Shopping</p>
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[344px] text-[14px] text-[red] text-right top-[390px] tracking-[-0.14px] w-[58px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱1,530
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[407px] tracking-[-0.14px] w-[33.677px]">13:50</p>
      <Group51 />
    </div>
  );
}

function Group15() {
  return (
    <div className="absolute contents left-[11px] top-[447px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[447px] w-[348px]" />
    </div>
  );
}

function Group45() {
  return (
    <div className="absolute contents left-[23px] top-[455px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[455px]" />
    </div>
  );
}

function Group16() {
  return (
    <div className="absolute inset-[140.73%_87.66%_-49.4%_8%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 16.2572 28.5343">
        <g id="Group">
          <path d={svgPaths.p1d140000} fill="var(--fill-0, white)" id="Vector" />
          <path d={svgPaths.p6cfea00} fill="var(--fill-0, white)" id="Vector_2" />
          <path d={svgPaths.p23c239b0} fill="var(--fill-0, white)" id="Vector_3" />
          <path d={svgPaths.p35862dc0} fill="var(--fill-0, white)" id="Vector_4" />
        </g>
      </svg>
    </div>
  );
}

function Group52() {
  return (
    <div className="absolute contents left-[23px] top-[455px]">
      <Group45 />
      <Group16 />
    </div>
  );
}

function Group44() {
  return (
    <div className="absolute contents left-[11px] top-[446px]">
      <Group15 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[453px] tracking-[-0.14px] w-[57px]">Grocery</p>
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[344px] text-[14px] text-[red] text-right top-[446px] tracking-[-0.14px] w-[56px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱3,250
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[463px] tracking-[-0.14px] w-[33.677px]">14:00</p>
      <Group52 />
    </div>
  );
}

function Group17() {
  return (
    <div className="absolute contents left-[11px] top-[502px]">
      <div className="absolute bg-white h-[45px] left-[11px] rounded-[13px] top-[502px] w-[348px]" />
    </div>
  );
}

function Group53() {
  return (
    <div className="absolute contents left-[23px] top-[510px]">
      <div className="absolute bg-[#01143d] left-[23px] rounded-[10px] size-[30px] top-[510px]" />
    </div>
  );
}

function Group18() {
  return (
    <div className="absolute inset-[157.45%_87.37%_-61.43%_8%]" data-name="Group">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 17.3729 13.1125">
        <g id="Group">
          <path d={svgPaths.p899ff80} fill="var(--fill-0, white)" id="Vector" />
          <path d={svgPaths.p1c15500} fill="var(--fill-0, white)" id="Vector_2" />
        </g>
      </svg>
    </div>
  );
}

function Group47() {
  return (
    <div className="absolute contents left-[23px] top-[510px]">
      <Group53 />
      <Group18 />
    </div>
  );
}

function Group46() {
  return (
    <div className="absolute contents left-[11px] top-[501px]">
      <Group17 />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[34px] left-[62px] not-italic text-[#01143d] text-[14px] top-[508px] tracking-[-0.14px] w-[57px]">Traffic</p>
      <Group47 />
      <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[343.69px] text-[14px] text-[red] text-right top-[501px] tracking-[-0.14px] w-[33.677px]" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
        -₱80
      </p>
      <p className="absolute font-['Poppins:Regular',sans-serif] leading-[34px] left-[310.01px] not-italic text-[#949292] text-[14px] top-[518px] tracking-[-0.14px] w-[33.677px]">14:30</p>
    </div>
  );
}

function Group50() {
  return (
    <div className="absolute contents left-[11px] top-0">
      <Group20 />
      <Group21 />
      <Group27 />
      <Group29 />
      <Group31 />
      <Group34 />
      <Group37 />
      <Group40 />
      <Group44 />
      <Group46 />
    </div>
  );
}

function History() {
  return (
    <div className="absolute h-[329px] left-[13px] overflow-x-clip overflow-y-auto top-[198px] w-[375px]" data-name="history">
      <Group50 />
    </div>
  );
}

function Group54() {
  return (
    <div className="absolute h-[50.474px] left-[313px] top-[487.58px] w-[50px]">
      <div className="absolute inset-[0_-8%_-15.85%_-8%]">
        <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 58 58.474">
          <g id="Group 75">
            <circle cx="28.9999" cy="20.4219" fill="var(--fill-0, white)" id="Ellipse 5" r="14" />
            <g filter="url(#filter0_d_5_629)" id="Vector">
              <path d={svgPaths.padc8680} fill="var(--fill-0, #295DC7)" />
            </g>
          </g>
          <defs>
            <filter colorInterpolationFilters="sRGB" filterUnits="userSpaceOnUse" height="58.4739" id="filter0_d_5_629" width="58" x="0" y="7.39098e-05">
              <feFlood floodOpacity="0" result="BackgroundImageFix" />
              <feColorMatrix in="SourceAlpha" result="hardAlpha" type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 127 0" />
              <feOffset dy="4" />
              <feGaussianBlur stdDeviation="2" />
              <feComposite in2="hardAlpha" operator="out" />
              <feColorMatrix type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0.25 0" />
              <feBlend in2="BackgroundImageFix" mode="normal" result="effect1_dropShadow_5_629" />
              <feBlend in="SourceGraphic" in2="effect1_dropShadow_5_629" mode="normal" result="shape" />
            </filter>
          </defs>
        </svg>
      </div>
    </div>
  );
}

function Group55() {
  return (
    <div className="absolute h-[53.41px] left-[21px] top-[19.59px] w-[85px]">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 85 53.4106">
        <g id="Group 71">
          <path d={svgPaths.p2f56cc0} fill="var(--fill-0, #E7EBEE)" id="Subtract" />
          <circle cx="42.5" cy="22.9104" fill="var(--fill-0, #01143D)" id="Ellipse 8" r="22.5" />
        </g>
      </svg>
    </div>
  );
}

export default function Dashboard() {
  return (
    <div className="bg-[#e7ebee] relative size-full" data-name="Dashboard">
      <div className="absolute bg-gradient-to-b from-[#01143d] from-[9%] h-[340px] left-[-10px] to-[#0335a3] top-[-2px] w-[413px]" />
      <div className="absolute bg-[#e7ebee] h-[633px] left-0 rounded-tl-[35px] rounded-tr-[35px] top-[222px] w-[393px]" />
      <div className="absolute left-[16px] size-[40px] top-[62px]">
        <img alt="" className="absolute block max-w-none size-full" height="40" src={imgEllipse3} width="40" />
      </div>
      <Group />
      <p className="absolute font-['Poppins:Medium',sans-serif] leading-[normal] left-[127px] not-italic text-[15px] text-[rgba(225,241,254,0.6)] top-[115px] whitespace-nowrap">{`Available balance `}</p>
      <p className="absolute font-['SF_Pro:Medium',sans-serif] font-[510] leading-[34px] left-[106px] text-[36px] text-white top-[150px] tracking-[-0.36px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        ₱10,867.50
      </p>
      <div className="absolute content-stretch flex h-[46px] items-center justify-between left-[-10px] overflow-clip px-[26px] py-[4px] top-[-2px] w-[420px]" data-name="Status bar and Menu bar- iPad">
        <Left />
        <Right />
      </div>
      <div className="absolute h-[639px] left-0 top-[213px] w-[393px]" data-name="dashboard">
        <Group19 />
        <p className="-translate-x-full absolute font-['Poppins:Regular','Noto_Sans:Regular',sans-serif] leading-[34px] left-[358px] text-[14px] text-[red] text-right top-[85px] tracking-[-0.14px] whitespace-nowrap" style={{ fontVariationSettings: "'CTGR' 0, 'wdth' 100, 'wght' 400" }}>
          ₱5,020
        </p>
        <div className="absolute bg-[#01143d] h-[43px] left-[24px] rounded-[20px] top-[147px] w-[348px]" />
        <p className="absolute font-['Poppins:SemiBold',sans-serif] leading-[34px] left-[40px] not-italic text-[16px] text-white top-[152px] tracking-[-0.16px] whitespace-nowrap">Sat, 13 September</p>
        <Group41 />
        <Group22 />
        <Group42 />
        <History />
        <Group54 />
      </div>
      <div className="absolute h-[94px] left-[22px] top-[726px] w-[348px]" data-name="navbar">
        <div className="absolute bg-[#01143d] inset-[37.23%_0_0_0] rounded-[25px]" />
        <div className="absolute bg-[#01143d] inset-[37.23%_0_0_0] rounded-[25px]" />
        <Group55 />
        <div className="absolute inset-[58.51%_15.23%_20.21%_79.02%]">
          <div aria-hidden="true" className="absolute inset-0 pointer-events-none">
            <img alt="" className="absolute max-w-none object-cover size-full" src={imgRectangle106} />
            <img alt="" className="absolute max-w-none object-cover size-full" src={imgRectangle106} />
          </div>
        </div>
        <div className="absolute inset-[58.51%_36.49%_20.21%_57.76%]">
          <img alt="" className="absolute inset-0 max-w-none object-cover pointer-events-none size-full" src={imgRectangle107} />
        </div>
        <div className="absolute inset-[58.51%_57.76%_20.21%_36.49%]">
          <img alt="" className="absolute inset-0 max-w-none object-cover pointer-events-none size-full" src={imgRectangle108} />
        </div>
        <div className="absolute inset-[35.11%_79.02%_43.62%_15.23%]">
          <img alt="" className="absolute inset-0 max-w-none object-cover pointer-events-none size-full" src={imgRectangle109} />
        </div>
      </div>
    </div>
  );
}