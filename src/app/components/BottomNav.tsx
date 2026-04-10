import { Home, List, PiggyBank, Settings } from 'lucide-react';
import { Link, useLocation } from 'react-router';

export default function BottomNav() {
  const location = useLocation();

  const isActive = (path: string) => location.pathname === path;

  return (
    <div className="fixed bottom-0 left-0 right-0 bg-[#01143D] rounded-t-[25px] shadow-lg">
      <div className="flex items-center justify-around max-w-md mx-auto px-6 py-4">
        <Link to="/dashboard" className={`flex flex-col items-center gap-1 ${isActive('/dashboard') ? 'text-[#00B2FF]' : 'text-gray-400'}`}>
          <Home className="w-6 h-6" />
        </Link>
        <Link to="/statistics" className={`flex flex-col items-center gap-1 ${isActive('/statistics') ? 'text-[#00B2FF]' : 'text-gray-400'}`}>
          <List className="w-6 h-6" />
        </Link>
        <Link to="/savings" className={`flex flex-col items-center gap-1 ${isActive('/savings') ? 'text-[#00B2FF]' : 'text-gray-400'}`}>
          <PiggyBank className="w-6 h-6" />
        </Link>
        <Link to="/settings" className={`flex flex-col items-center gap-1 ${isActive('/settings') ? 'text-[#00B2FF]' : 'text-gray-400'}`}>
          <Settings className="w-6 h-6" />
        </Link>
      </div>
    </div>
  );
}