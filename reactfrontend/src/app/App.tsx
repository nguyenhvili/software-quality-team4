import { Routes, Route } from "react-router-dom";
import { AppContextWrapper } from "./AppContextWrapper";

function App() {
  return (
    <AppContextWrapper>
      <Routes>
        <Route path="" element={<div>Hello world</div>} />
      </Routes>
    </AppContextWrapper>
  );
}

export default App;
