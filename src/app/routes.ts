import { createBrowserRouter } from "react-router";
import SplashScreen from "./screens/SplashScreen";
import GetStartedScreen from "./screens/GetStartedScreen";
import SignInScreen from "./screens/SignInScreen";
import SignUpScreen from "./screens/SignUpScreen";
import DashboardScreen from "./screens/DashboardScreen";
import StatisticsScreen from "./screens/StatisticsScreen";
import SavingsScreen from "./screens/SavingsScreen";
import DrawSaveScreen from "./screens/DrawSaveScreen";
import EditSavingPlanScreen from "./screens/EditSavingPlanScreen";
import AddTransactionScreen from "./screens/AddTransactionScreen";
import ProfileScreen from "./screens/ProfileScreen";
import SettingsScreen from "./screens/SettingsScreen";
import NotificationScreen from "./screens/NotificationScreen";

export const router = createBrowserRouter([
  {
    path: "/",
    Component: SplashScreen,
  },
  {
    path: "/get-started",
    Component: GetStartedScreen,
  },
  {
    path: "/signin",
    Component: SignInScreen,
  },
  {
    path: "/signup",
    Component: SignUpScreen,
  },
  {
    path: "/dashboard",
    Component: DashboardScreen,
  },
  {
    path: "/statistics",
    Component: StatisticsScreen,
  },
  {
    path: "/savings",
    Component: SavingsScreen,
  },
  {
    path: "/savings/:id",
    Component: DrawSaveScreen,
  },
  {
    path: "/savings/:id/edit",
    Component: EditSavingPlanScreen,
  },
  {
    path: "/add-transaction",
    Component: AddTransactionScreen,
  },
  {
    path: "/profile",
    Component: ProfileScreen,
  },
  {
    path: "/settings",
    Component: SettingsScreen,
  },
  {
    path: "/notifications",
    Component: NotificationScreen,
  },
]);
