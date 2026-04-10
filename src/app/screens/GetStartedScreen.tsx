import { useNavigate } from 'react-router';
import imgGetStarted from 'figma:asset/fc4bfc56a5e0926e226cf25b7cfb124d24bbcdcb.png';

export default function GetStartedScreen() {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen bg-white flex flex-col relative">
      {/* Main Content */}
      <div className="flex-1 flex flex-col items-center justify-center px-6 pt-16 pb-32">
        <img
          src={imgGetStarted}
          alt="Spendy App Preview"
          className="w-full max-w-[320px] mb-12"
        />

        <h1 className="text-[#01143D] text-[32px] font-bold text-center mb-3 leading-tight">
          Spendy – Budget Tracker
        </h1>

        <p className="text-gray-600 text-[16px] text-center px-4">
          Manage your money wisely with ease.
        </p>
      </div>

      {/* Bottom Section with curved background */}
      <div className="absolute bottom-0 left-0 right-0">
        {/* Curved background shape */}
        <div className="absolute inset-x-0 bottom-0 h-48 bg-[#01143D] rounded-t-[50px]" />
        
        {/* Button on top of background */}
        <div className="relative px-6 pb-12 pt-8">
          <button
            onClick={() => navigate('/signin')}
            className="w-full bg-[#0335a3] text-white py-4 rounded-full text-[18px] font-semibold shadow-lg hover:bg-[#022268] transition-colors"
          >
            Get Started
          </button>
        </div>
      </div>
    </div>
  );
}