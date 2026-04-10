import { useEffect } from 'react';
import { useNavigate } from 'react-router';

export default function SplashScreen() {
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setTimeout(() => {
      navigate('/get-started');
    }, 2000);
    return () => clearTimeout(timer);
  }, [navigate]);

  return (
    <div className="min-h-screen bg-[#01143D] flex items-center justify-center">
      <div className="text-center">
        <div className="text-white text-[120px] font-bold tracking-wider">S</div>
        <p className="text-white text-[18px] mt-4 tracking-wide">Spendy</p>
      </div>
    </div>
  );
}