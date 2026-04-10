import { ChevronLeft } from 'lucide-react';
import { useNavigate } from 'react-router';
import Notification from '../../imports/Notification';

export default function NotificationScreen() {
  const navigate = useNavigate();

  return (
    <div className="relative">
      <button
        onClick={() => navigate(-1)}
        className="absolute top-6 left-6 z-10 text-white"
      >
        <ChevronLeft className="w-6 h-6" />
      </button>
      <Notification />
    </div>
  );
}
