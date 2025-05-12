import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import svgrPlugin from 'vite-plugin-svgr';


// https://vitejs.dev/config/
export default defineConfig({
  base: '/software-quality-team4/',
  plugins: [plugin(), tailwindcss(), svgrPlugin()],
  build: {
    manifest: true,
  },
  server: {
    port: 3000,
  },
})
