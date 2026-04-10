import svgPaths from "./svg-u7ftzizy3o";
import imgEllipse6 from "figma:asset/69ca7ab92d67aa29944f86cbdd255cc6ff75a5f7.png";
import imgRectangle106 from "figma:asset/5aa2dba646fa4b1d5ac2510bb2e96d3cd62d4614.png";
import imgRectangle107 from "figma:asset/6f35f047681a0f54721d817c0de34838c5589b0a.png";
import imgRectangle108 from "figma:asset/1168208e4d9efd47fb1219e2552c19bdb3d11a17.png";
import imgRectangle109 from "figma:asset/184718e0c87fed565c40269ab917c082d4c7a79c.png";

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
            <mask fill="white" id="path-1-inside-1_5_1198">
              <path d={svgPaths.p303e8640} />
            </mask>
            <path d={svgPaths.p29c93100} fill="var(--stroke-0, white)" mask="url(#path-1-inside-1_5_1198)" />
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

function Group3() {
  return (
    <div className="absolute contents left-[-19px] top-[-20px]">
      <div className="absolute bg-gradient-to-b from-[#01143d] from-[9%] h-[381px] left-[-19px] to-[#0335a3] top-[-20px] w-[413px]" />
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

function Group4() {
  return (
    <div className="absolute contents left-[2px] top-[50px]">
      <div className="absolute bg-white h-[100px] left-[2px] rounded-[20px] shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] top-[50px] w-[345px]" />
      <p className="absolute font-['Poppins:Bold',sans-serif] leading-[34px] left-[21px] not-italic text-[#01143d] text-[15px] top-[55px] tracking-[-0.15px] whitespace-nowrap">Gaming PC</p>
      <p className="absolute font-['SF_Pro:Medium',sans-serif] font-[510] h-[20px] leading-[34px] left-[21px] text-[#01143d] text-[16px] top-[79px] tracking-[-0.16px] w-[152px]" style={{ fontVariationSettings: "'wdth' 100" }}>
        ₱30,000 / ₱40,000
      </p>
      <p className="-translate-x-full absolute font-['Poppins:Medium',sans-serif] leading-[normal] left-[332px] not-italic text-[#022268] text-[14px] text-right top-[61px] whitespace-nowrap">Edit</p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[169px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[120px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        Target Date: Dec 31, 2025
      </p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[335px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[91px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        90% Complete
      </p>
      <div className="absolute flex h-[0.994px] items-center justify-center left-[21px] top-[113.01px] w-[314px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.18deg]">
          <div className="h-0 relative w-[314.002px]">
            <div className="absolute inset-[-4px_0_0_0]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 314.002 4">
                <line id="Line 6" stroke="var(--stroke-0, #D9D9D9)" strokeLinecap="round" strokeWidth="4" x1="2" x2="312.002" y1="2" y2="2" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <div className="absolute flex h-[1.861px] items-center justify-center left-[22.99px] top-[110.8px] w-[232.507px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.34deg]">
          <div className="h-[0.473px] relative w-[232.508px]">
            <div className="absolute inset-[-423.21%_-0.86%_-423.17%_-0.86%]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 236.508 4.47237">
                <path d="M2 2L234.508 2.47237" id="Line 7" stroke="var(--stroke-0, #43B3EF)" strokeLinecap="round" strokeWidth="4" />
              </svg>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function Group7() {
  return (
    <div className="absolute contents left-[2px] top-[388px]">
      <div className="absolute bg-white h-[100px] left-[2px] rounded-[20px] shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] top-[388px] w-[345px]" />
      <p className="absolute font-['Poppins:Bold',sans-serif] leading-[34px] left-[21px] not-italic text-[#01143d] text-[15px] top-[393px] tracking-[-0.15px] whitespace-nowrap">Gaming PC</p>
      <p className="absolute font-['SF_Pro:Medium',sans-serif] font-[510] h-[20px] leading-[34px] left-[21px] text-[#01143d] text-[16px] top-[417px] tracking-[-0.16px] w-[152px]" style={{ fontVariationSettings: "'wdth' 100" }}>
        ₱30,000 / ₱40,000
      </p>
      <p className="-translate-x-full absolute font-['Poppins:Medium',sans-serif] leading-[normal] left-[332px] not-italic text-[#022268] text-[14px] text-right top-[399px] whitespace-nowrap">Edit</p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[169px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[458px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        Target Date: Dec 31, 2025
      </p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[335px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[429px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        90% Complete
      </p>
      <div className="absolute flex h-[0.994px] items-center justify-center left-[21px] top-[451.01px] w-[314px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.18deg]">
          <div className="h-0 relative w-[314.002px]">
            <div className="absolute inset-[-4px_0_0_0]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 314.002 4">
                <line id="Line 6" stroke="var(--stroke-0, #D9D9D9)" strokeLinecap="round" strokeWidth="4" x1="2" x2="312.002" y1="2" y2="2" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <div className="absolute flex h-[1.861px] items-center justify-center left-[22.99px] top-[448.8px] w-[232.507px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.34deg]">
          <div className="h-[0.473px] relative w-[232.508px]">
            <div className="absolute inset-[-423.21%_-0.86%_-423.17%_-0.86%]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 236.508 4.47237">
                <path d="M2 2L234.508 2.47237" id="Line 7" stroke="var(--stroke-0, #43B3EF)" strokeLinecap="round" strokeWidth="4" />
              </svg>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function Group8() {
  return (
    <div className="absolute contents left-[2px] top-[502px]">
      <div className="absolute bg-white h-[100px] left-[2px] rounded-[20px] shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] top-[502px] w-[345px]" />
      <p className="absolute font-['Poppins:Bold',sans-serif] leading-[34px] left-[21px] not-italic text-[#01143d] text-[15px] top-[507px] tracking-[-0.15px] whitespace-nowrap">Gaming PC</p>
      <p className="absolute font-['SF_Pro:Medium',sans-serif] font-[510] h-[20px] leading-[34px] left-[21px] text-[#01143d] text-[16px] top-[531px] tracking-[-0.16px] w-[152px]" style={{ fontVariationSettings: "'wdth' 100" }}>
        ₱30,000 / ₱40,000
      </p>
      <p className="-translate-x-full absolute font-['Poppins:Medium',sans-serif] leading-[normal] left-[332px] not-italic text-[#022268] text-[14px] text-right top-[513px] whitespace-nowrap">Edit</p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[169px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[572px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        Target Date: Dec 31, 2025
      </p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[335px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[543px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        90% Complete
      </p>
      <div className="absolute flex h-[0.994px] items-center justify-center left-[21px] top-[565.01px] w-[314px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.18deg]">
          <div className="h-0 relative w-[314.002px]">
            <div className="absolute inset-[-4px_0_0_0]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 314.002 4">
                <line id="Line 6" stroke="var(--stroke-0, #D9D9D9)" strokeLinecap="round" strokeWidth="4" x1="2" x2="312.002" y1="2" y2="2" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <div className="absolute flex h-[1.861px] items-center justify-center left-[22.99px] top-[562.8px] w-[232.507px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.34deg]">
          <div className="h-[0.473px] relative w-[232.508px]">
            <div className="absolute inset-[-423.21%_-0.86%_-423.17%_-0.86%]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 236.508 4.47237">
                <path d="M2 2L234.508 2.47237" id="Line 7" stroke="var(--stroke-0, #43B3EF)" strokeLinecap="round" strokeWidth="4" />
              </svg>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function Group6() {
  return (
    <div className="absolute contents left-[2px] top-[274px]">
      <div className="absolute bg-white h-[100px] left-[2px] rounded-[20px] shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] top-[274px] w-[345px]" />
      <p className="absolute font-['Poppins:Bold',sans-serif] leading-[34px] left-[21px] not-italic text-[#01143d] text-[15px] top-[279px] tracking-[-0.15px] whitespace-nowrap">Gaming PC</p>
      <p className="absolute font-['SF_Pro:Medium',sans-serif] font-[510] h-[20px] leading-[34px] left-[21px] text-[#01143d] text-[16px] top-[303px] tracking-[-0.16px] w-[152px]" style={{ fontVariationSettings: "'wdth' 100" }}>
        ₱30,000 / ₱40,000
      </p>
      <p className="-translate-x-full absolute font-['Poppins:Medium',sans-serif] leading-[normal] left-[332px] not-italic text-[#022268] text-[14px] text-right top-[285px] whitespace-nowrap">Edit</p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[169px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[344px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        Target Date: Dec 31, 2025
      </p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[335px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[315px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        90% Complete
      </p>
      <div className="absolute flex h-[0.994px] items-center justify-center left-[21px] top-[337.01px] w-[314px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.18deg]">
          <div className="h-0 relative w-[314.002px]">
            <div className="absolute inset-[-4px_0_0_0]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 314.002 4">
                <line id="Line 6" stroke="var(--stroke-0, #D9D9D9)" strokeLinecap="round" strokeWidth="4" x1="2" x2="312.002" y1="2" y2="2" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <div className="absolute flex h-[1.861px] items-center justify-center left-[22.99px] top-[334.8px] w-[232.507px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.34deg]">
          <div className="h-[0.473px] relative w-[232.508px]">
            <div className="absolute inset-[-423.21%_-0.86%_-423.17%_-0.86%]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 236.508 4.47237">
                <path d="M2 2L234.508 2.47237" id="Line 7" stroke="var(--stroke-0, #43B3EF)" strokeLinecap="round" strokeWidth="4" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <Group7 />
      <Group8 />
    </div>
  );
}

function Group5() {
  return (
    <div className="absolute contents left-px top-[162px]">
      <div className="absolute bg-white h-[100px] left-px rounded-[20px] shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] top-[162px] w-[345px]" />
      <p className="absolute font-['Poppins:Bold',sans-serif] leading-[34px] left-[21px] not-italic text-[#01143d] text-[15px] top-[176px] tracking-[-0.15px] whitespace-nowrap">Gaming PC</p>
      <p className="absolute font-['SF_Pro:Medium',sans-serif] font-[510] h-[20px] leading-[34px] left-[21px] text-[#01143d] text-[16px] top-[200px] tracking-[-0.16px] w-[152px]" style={{ fontVariationSettings: "'wdth' 100" }}>
        ₱30,000 / ₱40,000
      </p>
      <p className="-translate-x-full absolute font-['Poppins:Medium',sans-serif] leading-[normal] left-[332px] not-italic text-[#022268] text-[14px] text-right top-[182px] whitespace-nowrap">Edit</p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[169px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[241px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        Target Date: Dec 31, 2025
      </p>
      <p className="-translate-x-full absolute font-['SF_Pro:Regular',sans-serif] font-normal leading-[normal] left-[335px] text-[12px] text-[rgba(1,20,61,0.6)] text-right top-[212px] whitespace-nowrap" style={{ fontVariationSettings: "'wdth' 100" }}>
        90% Complete
      </p>
      <div className="absolute flex h-[0.994px] items-center justify-center left-[21px] top-[234.01px] w-[314px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.18deg]">
          <div className="h-0 relative w-[314.002px]">
            <div className="absolute inset-[-4px_0_0_0]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 314.002 4">
                <line id="Line 6" stroke="var(--stroke-0, #D9D9D9)" strokeLinecap="round" strokeWidth="4" x1="2" x2="312.002" y1="2" y2="2" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <div className="absolute flex h-[1.861px] items-center justify-center left-[22.99px] top-[231.8px] w-[232.507px]" style={{ "--transform-inner-width": "1185", "--transform-inner-height": "22" } as React.CSSProperties}>
        <div className="flex-none rotate-[-0.34deg]">
          <div className="h-[0.473px] relative w-[232.508px]">
            <div className="absolute inset-[-423.21%_-0.86%_-423.17%_-0.86%]">
              <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 236.508 4.47237">
                <path d="M2 2L234.508 2.47237" id="Line 7" stroke="var(--stroke-0, #43B3EF)" strokeLinecap="round" strokeWidth="4" />
              </svg>
            </div>
          </div>
        </div>
      </div>
      <Group6 />
    </div>
  );
}

function Group2() {
  return (
    <div className="absolute inset-[93.31%_1.44%_-1.47%_84.15%]">
      <div className="absolute inset-[0_-8%_-16%_-8%]">
        <svg className="block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 58 58">
          <g id="Group 77">
            <circle cx="29" cy="25" fill="var(--fill-0, white)" id="Ellipse 5" r="14" />
            <g filter="url(#filter0_d_5_1186)" id="Vector">
              <path d={svgPaths.p20c4a800} fill="var(--fill-0, #295DC7)" />
            </g>
          </g>
          <defs>
            <filter colorInterpolationFilters="sRGB" filterUnits="userSpaceOnUse" height="58" id="filter0_d_5_1186" width="58" x="0" y="0">
              <feFlood floodOpacity="0" result="BackgroundImageFix" />
              <feColorMatrix in="SourceAlpha" result="hardAlpha" type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 127 0" />
              <feOffset dy="4" />
              <feGaussianBlur stdDeviation="2" />
              <feComposite in2="hardAlpha" operator="out" />
              <feColorMatrix type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0.25 0" />
              <feBlend in2="BackgroundImageFix" mode="normal" result="effect1_dropShadow_5_1186" />
              <feBlend in="SourceGraphic" in2="effect1_dropShadow_5_1186" mode="normal" result="shape" />
            </filter>
          </defs>
        </svg>
      </div>
    </div>
  );
}

function Group1() {
  return (
    <div className="absolute h-[53.41px] left-[169px] top-[19.59px] w-[85px]">
      <svg className="absolute block size-full" fill="none" preserveAspectRatio="none" viewBox="0 0 85 53.4105">
        <g id="Group 71">
          <path d={svgPaths.p2131c3f0} fill="var(--fill-0, #E7EBEE)" id="Ellipse 8" />
          <circle cx="42.5" cy="22.9104" fill="var(--fill-0, #01143D)" id="Ellipse 8_2" r="22.5" />
        </g>
      </svg>
    </div>
  );
}

export default function Savings() {
  return (
    <div className="bg-[#e7ebee] relative size-full" data-name="Savings">
      <Group3 />
      <div className="absolute bg-[#e7ebee] h-[741px] left-0 rounded-tl-[30px] rounded-tr-[30px] top-[120px] w-[393px]" />
      <div className="absolute h-[613px] left-[22px] top-[133px] w-[347px]" data-name="Savings">
        <p className="absolute font-['Poppins:SemiBold',sans-serif] leading-[normal] left-0 not-italic text-[#01143d] text-[24px] top-0 whitespace-nowrap">Savings</p>
        <button className="-translate-x-full absolute block cursor-pointer font-['Poppins:Medium',sans-serif] leading-[0] left-[347px] not-italic text-[14px] text-[red] text-right top-[7px] whitespace-nowrap">
          <p className="leading-[normal]">Ended</p>
        </button>
        <Group4 />
        <Group5 />
        <Group2 />
      </div>
      <div className="absolute h-[94px] left-[22px] top-[726px] w-[348px]" data-name="navbar">
        <div className="absolute bg-[#01143d] inset-[37.23%_0_0_0] rounded-[25px]" />
        <Group1 />
        <div className="absolute inset-[58.51%_15.23%_20.21%_79.02%]">
          <div aria-hidden="true" className="absolute inset-0 pointer-events-none">
            <img alt="" className="absolute max-w-none object-cover size-full" src={imgRectangle106} />
            <img alt="" className="absolute max-w-none object-cover size-full" src={imgRectangle106} />
          </div>
        </div>
        <div className="absolute inset-[35.11%_36.49%_43.62%_57.76%]">
          <img alt="" className="absolute inset-0 max-w-none object-cover pointer-events-none size-full" src={imgRectangle107} />
        </div>
        <div className="absolute inset-[58.51%_58.05%_20.21%_36.21%]">
          <img alt="" className="absolute inset-0 max-w-none object-cover pointer-events-none size-full" src={imgRectangle108} />
        </div>
        <div className="absolute inset-[58.51%_79.6%_20.21%_14.66%]">
          <img alt="" className="absolute inset-0 max-w-none object-cover pointer-events-none size-full" src={imgRectangle109} />
        </div>
      </div>
    </div>
  );
}