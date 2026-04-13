import { defineConfig, type Plugin } from 'vite'
import path from 'path'
import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'

/** Figma Make exports `figma:asset/...` URLs; map them to a local PNG for Vite. */
function figmaAssetResolve(): Plugin {
  const placeholder = path.resolve(__dirname, 'src/assets/figma-placeholder.png')
  return {
    name: 'figma-asset-resolve',
    enforce: 'pre',
    resolveId(id) {
      if (id.startsWith('figma:asset/')) {
        return placeholder
      }
    },
  }
}

export default defineConfig({
  plugins: [
    figmaAssetResolve(),
    // The React and Tailwind plugins are both required for Make, even if
    // Tailwind is not being actively used – do not remove them
    react(),
    tailwindcss(),
  ],
  resolve: {
    alias: {
      // Alias @ to the src directory
      '@': path.resolve(__dirname, './src'),
    },
  },

  // File types to support raw imports. Never add .css, .tsx, or .ts files to this.
  assetsInclude: ['**/*.svg', '**/*.csv'],
})
