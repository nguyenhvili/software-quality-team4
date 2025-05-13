import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import svgrPlugin from 'vite-plugin-svgr';
import vercel from 'vite-plugin-vercel';


// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin(), tailwindcss(), svgrPlugin(), vercel()],
    build: {
        manifest: true,
      },
      server: {
        port: process.env.PORT,
      },
})
